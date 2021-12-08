import 'package:brenda_wallet/api/carol_client.dart';
import 'package:brenda_wallet/data/img.dart';
import 'package:brenda_wallet/data/my_colors.dart';
import 'package:brenda_wallet/models/user_send.dart';
import 'package:brenda_wallet/routes/brenda_send_transfer.dart';
import 'package:brenda_wallet/widget/my_text.dart';
import 'package:flutter/material.dart';
import 'package:flutter_masked_text/flutter_masked_text.dart';

class BrendaSendHandleRoute extends StatefulWidget {
  final String qrCode;

  const BrendaSendHandleRoute(this.qrCode);

  @override
  BrendaSendHandleRouteState createState() => new BrendaSendHandleRouteState();
}

class BrendaSendHandleRouteState extends State<BrendaSendHandleRoute> {
  BuildContext _scaffoldCtx;
  final userSendValue = new MoneyMaskedTextController(
      leftSymbol: 'BRE\$ ', decimalSeparator: '.', thousandSeparator: ',');

  Future<UserSend> userSend;

  @override
  void initState() {
    super.initState();
    userSend = getUserSend(widget.qrCode);
  }

  Future<UserSend> getUserSend(String userId) async {
    try {
      return await CarolClient().getUserSend(userId);
    } catch (e) {
      Navigator.pop(context);
    }

    return null;
  }

  @override
  Widget build(BuildContext context) {
    _scaffoldCtx = context;
    return Scaffold(
        backgroundColor: Colors.blueAccent[400],
        body: FutureBuilder<UserSend>(
            future: userSend,
            builder: (context, snapshot) {
              if (snapshot.hasData) {
                return sendBrenda(context, snapshot.data);
              } else
                return Center(
                  child: CircularProgressIndicator(),
                );
            }));
  }

  Widget sendBrenda(BuildContext context, UserSend userSendInfo) {
    return LayoutBuilder(builder: (context, constraint) {
      return SingleChildScrollView(
        padding: EdgeInsets.all(40),
        child: ConstrainedBox(
          constraints: BoxConstraints(minHeight: constraint.maxHeight),
          child: IntrinsicHeight(
            child: Column(
              //mainAxisSize: MainAxisSize.min,
              children: <Widget>[
                //Container(height: 40),
                Image.asset(Img.get('logo_small.png')),
                Text("Transaferência Brenda",
                    style: MyText.title(context).copyWith(color: Colors.white)),
                Container(height: 10),
                Text(
                    "Confirme o endereço de email e telefone para quem você enviando suas Brendas e digite abaixo o valorda transferência: ",
                    textAlign: TextAlign.center,
                    style: MyText.medium(context)
                        .copyWith(color: MyColors.grey_10)),
                Container(height: 10),
                Text(
                  userSendInfo.email,
                  textAlign: TextAlign.center,
                  style: MyText.title(context).copyWith(color: Colors.white),
                ),
                Container(height: 5),
                Text(
                  userSendInfo.phoneNumber,
                  textAlign: TextAlign.center,
                  style: MyText.title(context).copyWith(color: Colors.white),
                ),
                Container(height: 20),
                Container(
                  child: TextField(
                    controller: userSendValue,
                    style: TextStyle(color: Colors.white, fontSize: 30),
                    textAlign: TextAlign.center,
                    keyboardType: TextInputType.number,
                    cursorColor: Colors.white,
                    decoration: InputDecoration(
                      enabledBorder: UnderlineInputBorder(
                        borderSide: BorderSide(color: Colors.white, width: 1),
                      ),
                      focusedBorder: UnderlineInputBorder(
                        borderSide: BorderSide(color: Colors.white, width: 2),
                      ),
                    ),
                  ),
                ),
                Container(height: 20),
                Text("Tudo pronto?",
                    textAlign: TextAlign.center,
                    style: TextStyle(color: Colors.white)),
                Container(
                  height: 30,
                  child: FlatButton(
                    child: Text("Enviar ${userSendValue.value.text}",
                        style: MyText.subhead(context).copyWith(
                            color: Colors.white,
                            fontWeight: FontWeight.bold,
                            fontSize: 20)),
                    color: Colors.transparent,
                    onPressed: () =>
                        sendBrendas(userSendInfo, userSendValue.text),
                  ),
                ),
                Container(height: 50),
              ],
            ),
          ),
        ),
      );
    });
  }

  Future sendBrendas(UserSend userSend, String value) async {
    var valueDouble = double.parse(value.replaceAll('BRE\$', '').trim());
    Navigator.of(_scaffoldCtx).pushReplacement(MaterialPageRoute(
        builder: (_scaffoldCtx) =>
            BrendaSendTransferRoute(userSend, valueDouble)));
  }
}
