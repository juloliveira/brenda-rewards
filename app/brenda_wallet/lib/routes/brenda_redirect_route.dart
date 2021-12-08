import 'package:brenda_wallet/models/voucher.dart';
import 'package:flutter/material.dart';
import 'package:webview_flutter/webview_flutter.dart';

import 'brenda_reward.dart';

class BrendaRedirectRoute extends StatefulWidget {
  final Voucher voucher;

  BrendaRedirectRoute(this.voucher);

  @override
  BrendaRedirectRouteState createState() => new BrendaRedirectRouteState();
}

class BrendaRedirectRouteState extends State<BrendaRedirectRoute> {
  @override
  void dispose() {
    print("!========================================> DISPOSE REDIRECT ROUTE!");
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return WillPopScope(
      onWillPop: _onBackPressed,
      child: Scaffold(
          body: WebView(
        initialUrl: widget.voucher.campaign.resource,
        javascriptMode: JavascriptMode.disabled,
        gestureNavigationEnabled: true,
      )),
    );
  }

  Future<bool> _onBackPressed() async {
    print("!========================================> WAS POPPED!");
    Navigator.of(context).pushReplacement(MaterialPageRoute(builder: (context) {
      return BrendaRewardRoute(widget.voucher);
    }));

    return true;
  }
}
