import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:permission_handler/permission_handler.dart';

import 'api/token_client.dart';
import 'contants/keys.dart';
import 'data/img.dart';
import 'data/my_colors.dart';
import 'database/database.dart';
import 'widget/my_text.dart';

class SplashScreen extends StatefulWidget {
  SplashScreen({Key key}) : super(key: key);

  @override
  SplashScreenState createState() {
    return SplashScreenState();
  }
}

class SplashScreenState extends State<SplashScreen> {
  Future<StatefulWidget> token;
  final _storage = FlutterSecureStorage();

  @override
  void initState() {
    super.initState();
    checkPermissions();
    Timer.run(() async {
      await checkToken();
    });
  }

  checkPermissions() async {
    await [
      Permission.camera,
      Permission.locationWhenInUse,
      Permission.microphone,
      Permission.notification,
    ].request();
  }

  Future checkToken() async {
    var expirationData = await _storage.read(key: Keys.TOKEN_EXPIRATION);
    if (expirationData != null) {
      var expiration = DateTime.parse(expirationData + 'Z');
      if (expiration.difference(DateTime.now().toUtc()).inSeconds > 0) {
        return Navigator.pushReplacementNamed(context, '/BrendaWallet');
      } else {
        var username = await _storage.read(key: Keys.USERNAME);
        var refreshToken = await _storage.read(key: Keys.REFRESH_TOKEN);
        var db = Db();
        try {
          token = await TokenClient()
              .refreshToken(username, refreshToken)
              .then((token) async => await db.saveToken(username, token));
          return Navigator.pushReplacementNamed(context, '/BrendaWallet');
        } catch (RefreshTokenUnauthorizedException) {
          await db.clear();
          return Navigator.pushReplacementNamed(context, '/BrendaSignIn');
        }
      }
    }

    return Navigator.pushReplacementNamed(context, '/BrendaSignIn');
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(backgroundColor: Colors.white, body: splashWidget());
  }

  Widget splashWidget() {
    return Align(
      child: Container(
        width: 205,
        height: 350,
        alignment: Alignment.center,
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: <Widget>[
            Container(
              child: Image.asset(Img.get('logo_small_round.png'),
                  fit: BoxFit.cover),
            ),
            Container(height: 10),
            Text("Brenda Rewards",
                style: MyText.headline(context).copyWith(
                    color: Colors.grey[800], fontWeight: FontWeight.w600)),
            Text("Plataforma de Recompensas",
                style: MyText.body1(context).copyWith(color: Colors.grey[500])),
            Container(height: 20),
            Container(
              height: 5,
              width: 80,
              child: LinearProgressIndicator(
                valueColor:
                    AlwaysStoppedAnimation<Color>(MyColors.primaryLight),
                backgroundColor: Colors.grey[300],
              ),
            ),
          ],
        ),
      ),
      alignment: Alignment.center,
    );
  }
}
