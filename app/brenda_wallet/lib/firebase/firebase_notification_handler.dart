import 'dart:io';
import 'package:brenda_wallet/api/user_client.dart';
import 'package:firebase_messaging/firebase_messaging.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class FirebaseNotifications {
  FirebaseMessaging _firebaseMessaging;

  final void Function(double) updateBalance;

  FirebaseNotifications(this.updateBalance);

  void setUpFirebase() {
    _firebaseMessaging = FirebaseMessaging();
    _firebaseCloudMessagingListeners();
  }

  void _firebaseCloudMessagingListeners() {
    if (Platform.isIOS) _iOSPermission();

    _firebaseMessaging.getToken().then((token) async {
      final storage = FlutterSecureStorage();
      await UserClient().updateFirebaseToken(token);
      await storage.write(key: 'firebase_token', value: token);

      print("token: $token");
    });

    _firebaseMessaging.configure(
      onMessage: (Map<String, dynamic> message) async {
        print("on message $message");

        for (var keys in message.keys) {
          if (keys == "data") {
            print('$keys was written by ${message[keys]}');
            String valueData = message[keys].toString();
            if (valueData.indexOf("updated_balance") > 0) {
              updateBalance(double.parse(message[keys]['updated_balance']));
            }
          }
        }
      },
      onResume: (Map<String, dynamic> message) async {},
      onLaunch: (Map<String, dynamic> message) async {},
    );
  }

  void _iOSPermission() {
    _firebaseMessaging.requestNotificationPermissions(
        IosNotificationSettings(sound: true, badge: true, alert: true));
    _firebaseMessaging.onIosSettingsRegistered
        .listen((IosNotificationSettings settings) {
      print("Settings registered: $settings");
    });
  }
}
