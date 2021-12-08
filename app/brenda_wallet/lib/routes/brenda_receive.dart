import 'package:flutter/material.dart';
import 'package:qr_flutter/qr_flutter.dart';

class BrendaReceiveRoute extends StatefulWidget {
  @override
  BrendaReceiveRouteState createState() => new BrendaReceiveRouteState();
}

class BrendaReceiveRouteState extends State<BrendaReceiveRoute> {
  @override
  Widget build(BuildContext context) {
    final String userId = ModalRoute.of(context).settings.arguments;
    final String code = "http:\/\/bren.kr\/${userId.toUpperCase()}";
    return MaterialApp(
      title: 'Brenda Rewards',
      theme: ThemeData.light(),
      debugShowCheckedModeBanner: false,
      home: Material(
        color: Colors.white,
        child: SafeArea(
          top: true,
          bottom: true,
          child: Container(
            child: Column(
              children: <Widget>[
                Expanded(
                  child: Center(
                    child: Container(
                      width: 280,
                      child: CustomPaint(
                        size: Size.square(280),
                        painter: QrPainter(
                          data: code,
                          version: QrVersions.auto,
                          // size: 320.0,
                        ),
                      ),
                    ),
                  ),
                ),
                Padding(
                  padding: EdgeInsets.symmetric(vertical: 20, horizontal: 40)
                      .copyWith(bottom: 40),
                  child: Text("Este é seu código para receber Brendas."),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
