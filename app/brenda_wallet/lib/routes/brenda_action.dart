import 'package:brenda_wallet/api/campaigns.dart';
import 'package:brenda_wallet/exceptions/campaign_rewarded.dart';
import 'package:brenda_wallet/models/brenda_gps.dart';
import 'package:brenda_wallet/models/voucher.dart';
import 'package:brenda_wallet/routes/brenda_watch.dart';
import 'package:flutter/material.dart';
import 'package:toast/toast.dart';
import 'package:brenda_wallet/contants/actions.dart' as Actions;

import 'brenda_quiz_route.dart';
import 'brenda_redirect_route.dart';

class BrendaActionRoute extends StatefulWidget {
  final BrendaTag tag;
  final String deviceId;
  final String deviceData;

  const BrendaActionRoute(this.tag, this.deviceId, this.deviceData);

  @override
  BrendaActionRouteState createState() => new BrendaActionRouteState();
}

class BrendaActionRouteState extends State<BrendaActionRoute> {
  Future<Voucher> _voucher;

  @override
  void initState() {
    super.initState();

    _voucher = getData(widget.tag, widget.deviceId, widget.deviceData);
  }

  @override
  void dispose() {
    print("!=========================================> DISPOSE ACTION ROUTE!");
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: FutureBuilder<Voucher>(
          future: _voucher,
          builder: (context, snapshot) {
            if (snapshot.hasData) {
              switch (snapshot.data.campaign.action) {
                case Actions.REDIRECT:
                  return BrendaRedirectRoute(snapshot.data);
                case Actions.VIDEO:
                  return BrendaWatchRoute(snapshot.data);
                case Actions.QUIZ:
                  return BrendaQuizRoute(snapshot.data);
                default:
                  throw new Exception("Action not implemented");
                  break;
              }
            } else
              return Center(
                child: CircularProgressIndicator(),
              );
          }),
    );
  }

  Future<Voucher> getData(
      BrendaTag brendaTag, String deviceId, String deviceData) async {
    try {
      return await CampaignClient()
          .getCampaign(brendaTag, deviceId, deviceData);
    } on RefreshTokenUnauthorizedException catch (e) {
      Navigator.pushReplacementNamed(context, '/BrendaSignIn');
    } on ApiException catch (e) {
      Navigator.pop(context);
      Toast.show(e.message, context, duration: 7);
    }

    return null;
  }
}
