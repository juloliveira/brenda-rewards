import 'package:brenda_wallet/routes/brenda_send_handle.dart';
import 'package:flutter/material.dart';
import 'package:qr_mobile_vision/qr_camera.dart';
import 'package:qr_mobile_vision/qr_mobile_vision.dart';
import 'package:toast/toast.dart';

class BrendaSendRoute extends StatefulWidget {
  @override
  BrendaSendRouteState createState() => new BrendaSendRouteState();
}

class BrendaSendRouteState extends State<BrendaSendRoute> {
  BuildContext _scaffoldCtx;
  bool canScan = true;
  bool validSendCode = false;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: LayoutBuilder(
          builder: (BuildContext context, BoxConstraints constraints) {
        _scaffoldCtx = context;
        return aux();
      }),
    );
  }

  Widget aux() {
    return Container(
      child: Center(
        child: QrCamera(
          formats: [BarcodeFormats.QR_CODE],
          qrCodeCallback: (String value) {
            if (canScan) {
              setState(() {
                canScan = false;
              });

              if (validateScanData(value)) {
                Navigator.of(_scaffoldCtx).pushReplacement(MaterialPageRoute(
                    builder: (_scaffoldCtx) =>
                        BrendaSendHandleRoute(value.substring(15))));
              } else {
                Navigator.pop(_scaffoldCtx);
                Toast.show("Código de envio inválido.", _scaffoldCtx);
              }
            }
          },
          //qrCodeCallback: handleScan,
        ),
      ),
    );
  }

  bool validateScanData(String value) {
    final regexp = RegExp(r"http:\/\/bren.kr\/[A-F0-9\-]{36,36}$");
    return regexp.hasMatch(value);
  }
}
