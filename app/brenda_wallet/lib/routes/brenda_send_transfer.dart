import 'package:brenda_wallet/api/carol_client.dart';
import 'package:brenda_wallet/models/user_send.dart';
import 'package:brenda_wallet/models/user_transfer_brenda.dart';
import 'package:brenda_wallet/widget/my_text.dart';
import 'package:flutter/material.dart';

class BrendaSendTransferRoute extends StatefulWidget {
  final UserSend userSend;
  final double userTransferValue;

  const BrendaSendTransferRoute(this.userSend, this.userTransferValue);

  @override
  BrendaSendTransferRouteState createState() =>
      new BrendaSendTransferRouteState();
}

class BrendaSendTransferRouteState extends State<BrendaSendTransferRoute> {
  Future<UserTransferBrenda> userTransfer;

  @override
  void initState() {
    super.initState();
    userTransfer = postUserTransfer(widget.userSend, widget.userTransferValue);
  }

  Future<UserTransferBrenda> postUserTransfer(
      UserSend userSend, double userTransferValue) {
    return CarolClient().postUserTransfer(userSend, userTransferValue);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        backgroundColor: Colors.blueAccent[400],
        body: FutureBuilder<UserTransferBrenda>(
            future: userTransfer,
            builder: (context, snapshot) {
              if (snapshot.hasData) {
                return transferedBrenda(context, snapshot.data);
              } else
                return Center(
                  child: CircularProgressIndicator(),
                );
            }));
  }

  Widget transferedBrenda(
      BuildContext context, UserTransferBrenda userTransferBrenda) {
    return Center(
      child: Text(
        "Sua solicitação de transaferência foi concluída.",
        style: MyText.display1(context).copyWith(color: Colors.white),
      ),
    );
  }
}
