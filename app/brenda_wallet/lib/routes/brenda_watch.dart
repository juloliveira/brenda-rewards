import 'package:brenda_wallet/models/voucher.dart';
import 'package:brenda_wallet/routes/brenda_reward.dart';
import 'package:chewie/chewie.dart';
import 'package:flutter/material.dart';
import 'package:video_player/video_player.dart';
import 'package:wakelock/wakelock.dart';

class BrendaWatchRoute extends StatefulWidget {
  final Voucher voucher;

  BrendaWatchRoute(this.voucher);

  @override
  BrendaWatchRouteState createState() => new BrendaWatchRouteState();
}

class BrendaWatchRouteState extends State<BrendaWatchRoute> {
  BuildContext _scaffoldCtx;

  VideoPlayerController _videoPlayerController;
  ChewieController _chewieController;

  int check = 0;
  List<int> checked = List<int>();
  int ok = 0;

  @override
  void initState() {
    super.initState();
    _videoPlayerController =
        VideoPlayerController.network(widget.voucher.campaign.resource);
    _chewieController = ChewieController(
      videoPlayerController: _videoPlayerController,
      autoInitialize: true,
      autoPlay: true,
      fullScreenByDefault: true,
      //allowFullScreen: true,
      showControlsOnInitialize: false,
      allowMuting: false,
      showControls: false,
    );

    _chewieController.videoPlayerController.addListener(checkVideo);

    Wakelock.enable();
  }

  @override
  void dispose() {
    _videoPlayerController.dispose();
    _chewieController.dispose();
    Wakelock.disable();

    print("DISPOSE! #######################################################");

    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    _scaffoldCtx = context;
    return Scaffold(
        body: Center(
            child: Chewie(
      controller: _chewieController,
    )));
  }

  void checkVideo() {
    var player = _chewieController.videoPlayerController;
    if (player.value.position == null) return;

    if (ok == 0 && player.value.position.inSeconds > 0) {
      setState(() {
        for (var i = 1; i <= player.value.duration.inSeconds; i++) {
          ok += i;
        }
        //print("Ok Calculated: $ok");
      });
    }

    if (!checked.any((i) => i == player.value.position.inSeconds)) {
      setState(() {
        var pos = player.value.position.inSeconds;
        checked.add(pos);
        check += player.value.position.inSeconds;
        //print("Add position: $pos");

        //if (pos >= 1) goToReward();
      });
    }

    if (player.value.position == player.value.duration) {
      player.removeListener(checkVideo);
      //var percent = (check / ok);
      //var watchAll = (percent >= 0.85);

      //print("Chegou ao fim com $percent %");
      //Navigator.pop(context, watchAll);
      _chewieController.exitFullScreen();
      goToReward();
    }

    // var percent = (check / ok) * 100;
    // print("Watch: $percent %");
  }

  goToReward() {
    Navigator.of(_scaffoldCtx)
        .pushReplacement(MaterialPageRoute(builder: (context) {
      return BrendaRewardRoute(widget.voucher);
    }));
  }
}
