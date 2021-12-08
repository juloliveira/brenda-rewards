import 'package:brenda_wallet/api/campaigns.dart';
import 'package:brenda_wallet/data/img.dart';
import 'package:brenda_wallet/models/reward_message.dart';
import 'package:brenda_wallet/models/voucher.dart';
import 'package:brenda_wallet/widget/my_text.dart';
import 'package:flutter/material.dart';

class BrendaRewardRoute extends StatefulWidget {
  final Voucher voucher;

  const BrendaRewardRoute(this.voucher);

  @override
  BrendaRewardRouteState createState() => new BrendaRewardRouteState();
}

class BrendaRewardRouteState extends State<BrendaRewardRoute> {
  Future<RewardMessage> _reward;

  @override
  void initState() {
    _reward = CampaignClient().getReward(widget.voucher);
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: FutureBuilder<RewardMessage>(
        future: _reward,
        builder: (context, snapshot) {
          if (snapshot.hasData) {
            return reward(snapshot.data);
          } else
            return Center(
              child: CircularProgressIndicator(),
            );
        },
      ),
    );
  }

  Widget reward(RewardMessage brenda) {
    return Container(
      width: double.infinity,
      height: double.infinity,
      padding: EdgeInsets.only(top: 20),
      child: Container(
          padding: EdgeInsets.all(20),
          width: 280,
          height: 370,
          child: Card(
            shape:
                RoundedRectangleBorder(borderRadius: BorderRadius.circular(3)),
            clipBehavior: Clip.antiAliasWithSaveLayer,
            elevation: 2,
            child: Stack(
              children: <Widget>[
                Container(color: Colors.blueAccent[400].withOpacity(0.7)),
                Column(
                  children: <Widget>[
                    Container(height: 35),
                    Text("Oferecimento",
                        style: MyText.subhead(context).copyWith(
                            color: Colors.white, fontWeight: FontWeight.bold)),
                    Container(height: 10),
                    Text(brenda.customer,
                        style: MyText.title(context).copyWith(
                            color: Colors.white, fontWeight: FontWeight.bold)),
                    Container(
                      margin:
                          EdgeInsets.symmetric(vertical: 10, horizontal: 25),
                      child: Text(
                          "${brenda.customer} premiou você com ${brenda.reward} pela sua atenção. ",
                          textAlign: TextAlign.center,
                          style: MyText.subhead(context)
                              .copyWith(color: Colors.white)),
                    ),
                    Expanded(
                      child: brenda.logo == null
                          ? Image.asset(Img.get('logo_small.png'), width: 150)
                          : Image.network(brenda.logo + "?amanha"),
                    ),
                  ],
                )
              ],
            ),
          )),
    );
  }
}
