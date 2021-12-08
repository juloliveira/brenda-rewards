import 'dart:async';
import 'dart:convert';
import 'dart:io';

import 'package:brenda_wallet/api/carol_client.dart';
import 'package:brenda_wallet/contants/keys.dart';
import 'package:brenda_wallet/firebase/firebase_notification_handler.dart';
import 'package:brenda_wallet/models/brenda_gps.dart';
import 'package:brenda_wallet/models/share_in.dart';
import 'package:brenda_wallet/models/user.dart';
import 'package:brenda_wallet/routes/brenda_action.dart';
import 'package:device_info/device_info.dart';
import 'package:flutter/material.dart';
import 'package:brenda_wallet/data/img.dart';
import 'package:brenda_wallet/data/my_colors.dart';
import 'package:brenda_wallet/widget/my_text.dart';
import 'package:flutter/services.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:geolocator/geolocator.dart';
import 'package:intl/date_symbol_data_local.dart';
import 'package:intl/intl.dart';

import 'package:qr_mobile_vision/qr_camera.dart';
import 'package:qr_mobile_vision/qr_mobile_vision.dart';
import 'package:receive_sharing_intent/receive_sharing_intent.dart';
import 'package:share/share.dart';
import 'package:toast/toast.dart';

class BrendaWalletRoute extends StatefulWidget {
  BrendaWalletRoute();

  @override
  BrendaWalletRouteState createState() => new BrendaWalletRouteState();
}

class BrendaWalletRouteState extends State<BrendaWalletRoute> {
  static final DeviceInfoPlugin deviceInfoPlugin = DeviceInfoPlugin();
  final _storage = FlutterSecureStorage();
  String _deviceId;
  String _deviceData;

  BrendaGps gps = BrendaGps();

  PersistentBottomSheetController sheetController;
  BuildContext _scaffoldCtx;
  bool showSheet = false;
  bool canScan = false;

  GlobalKey<ScaffoldState> scaffoldKey = GlobalKey<ScaffoldState>();

  final _currency = new NumberFormat("#,##0.00", "pt_BR");

  Future<User> _user;
  String _userId;
  double _balance;
  Timer _timerPosition;

  List<dynamic> transactions = new List<dynamic>();

  StreamSubscription _intentDataStreamSubscription;

  final _shareIn = new ShareIn();

  @override
  void initState() {
    super.initState();
    initPlatformState();
    new FirebaseNotifications(updateBalance).setUpFirebase();

    initializeDateFormatting('pt_BR', null).then((_) {
      _user = CarolClient().getUserData();
    });

    _storage.read(key: Keys.USER_ID).then((value) {
      setState(() {
        _userId = value;
      });
    });

    _getCurrentPosition();
    _timerPosition = Timer.periodic(Duration(seconds: 7), (timer) {
      try {
        _getCurrentPosition();
      } catch (e) {}
    });

    _shareIn.addListener(handleShareUri, ['uri']);

    _intentDataStreamSubscription =
        ReceiveSharingIntent.getTextStream().listen((String value) {
      setState(() {
        _shareIn.uri = value;
      });
    }, onError: (err) {
      print("getLinkStream error: $err");
    });

    ReceiveSharingIntent.getInitialText().then((String value) {
      if (value == null) return;
      Toast.show("App fechada", context, duration: 30000);
      setState(() {
        _shareIn.uri = value;
      });
    });
  }

  @override
  void dispose() {
    print("!=========================================> DISPOSE WALLET ROUTE!");
    _timerPosition.cancel();
    _intentDataStreamSubscription.cancel();
    super.dispose();
  }

  void handleShareUri() {
    final regexp = RegExp(
        r"^http(s)??\:\/\/?(www\.)?((uau\.tw\/)|(youtu.be\/))([a-zA-Z0-9\-_])+");
    if (!regexp.hasMatch(_shareIn.uri)) {
      Toast.show("O conteúdo compartilhado não foi reconhecido.", context);
      return;
    }

    _getCurrentPosition().then((value) {
      print("Abriu a Brenda e chegamos aqui.");
      Navigator.push(
          context,
          MaterialPageRoute(
              builder: (context) => BrendaActionRoute(
                  gps.withTag(_shareIn.uri), _deviceId, _deviceData)));
    });
  }

  void updateBalance(double balance) {
    setState(() {
      _user = CarolClient().getUserData();
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        body: FutureBuilder<User>(
            future: _user,
            builder: (context, snapshot) {
              if (snapshot.hasData) {
                _balance = snapshot.data.balance;

                return dashboard(context, snapshot.data);
              } else
                return Center(
                  child: CircularProgressIndicator(),
                );
            }));
  }

  void refresh() {
    //Toast.show("Teste", context);
  }

  Widget dashboard(BuildContext context, User user) {
    return new Scaffold(
      key: scaffoldKey,
      backgroundColor: Colors.blueAccent[400],
      body: LayoutBuilder(
        builder: (BuildContext context, BoxConstraints constraints) {
          _scaffoldCtx = context;
          return Container(
            height: constraints.maxHeight,
            padding: EdgeInsets.only(left: 15, right: 15),
            child: Column(
              children: [
                appBar(),
                balance(user),
                sendReceive(),
                Container(height: 10),
                Text("Últimas Transações",
                    style: MyText.body2(context).copyWith(color: Colors.white)),
                Container(height: 5),
                Expanded(
                  child: Container(
                    height: constraints.maxHeight / 2,
                    child: ListView.builder(
                      padding: EdgeInsets.all(0),
                      scrollDirection: Axis.vertical,
                      itemCount: user.transactions.length,
                      itemBuilder: (BuildContext context, int index) {
                        return GestureDetector(
                          onLongPress: () {
                            Share.share(
                                'http://uau.tw/E7692587 \r\nQuer ganhar Brendas, a moeda virtual do mercado publicitário? Clica no link, você ganha e eu também. Legal, né?');
                          },
                          child: Card(
                            shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(6)),
                            color: Colors.white,
                            elevation: 2,
                            clipBehavior: Clip.antiAliasWithSaveLayer,
                            child: Container(
                              padding: EdgeInsets.all(15),
                              child: Row(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: <Widget>[
                                  CircleAvatar(
                                    radius: 12,
                                    backgroundColor: MyColors.grey_10,
                                    child: Icon(Icons.arrow_downward,
                                        color: MyColors.grey_40, size: 15),
                                  ),
                                  Container(width: 15),
                                  Column(
                                    crossAxisAlignment:
                                        CrossAxisAlignment.start,
                                    children: <Widget>[
                                      Container(
                                          width: 100,
                                          child: Text(
                                              user.transactions
                                                      .elementAt(index)
                                                      .customer ??
                                                  user.transactions
                                                      .elementAt(index)
                                                      .title,
                                              style: MyText.subhead(context)
                                                  .copyWith(
                                                      color: Colors.green[900],
                                                      fontWeight:
                                                          FontWeight.w500))),
                                      Container(height: 5),
                                      Container(
                                          width: 150,
                                          child: Text(
                                              user.transactions
                                                  .elementAt(index)
                                                  .description,
                                              style: MyText.caption(context)
                                                  .copyWith(
                                                      color:
                                                          MyColors.grey_40))),
                                    ],
                                  ),
                                  Spacer(),
                                  Column(
                                    crossAxisAlignment: CrossAxisAlignment.end,
                                    children: <Widget>[
                                      Text(
                                          "BRE\$ " +
                                              _currency.format(user.transactions
                                                  .elementAt(index)
                                                  .value),
                                          style: MyText.body1(context).copyWith(
                                              color: Colors.green[500])),
                                      Container(height: 5),
                                      Text(
                                          DateFormat("d MMM", 'pt_BR').format(
                                              user.transactions
                                                  .elementAt(index)
                                                  .createdAt
                                                  .toLocal()),
                                          style: MyText.caption(context)
                                              .copyWith(
                                                  color: MyColors.grey_40)),
                                    ],
                                  )
                                ],
                              ),
                            ),
                          ),
                        );
                      },
                    ),
                  ),
                )
              ],
            ),
          );
        },
      ),
      floatingActionButton: FloatingActionButton(
          heroTag: "camera",
          backgroundColor: Colors.pink[500],
          elevation: 10,
          child: Image.asset(
            Img.get("ic_qrcode30.png"),
            color: Colors.white,
          ),
          onPressed: () {
            setState(() {
              showSheet = !showSheet;
              canScan = true;
              if (showSheet) {
                _showScanner();
              } else {
                Navigator.pop(_scaffoldCtx);
              }
            });
          }),
      drawer: Drawer(
        child: SingleChildScrollView(
          child: Column(
            children: <Widget>[
              Container(
                height: 190,
                child: Stack(
                  children: <Widget>[
                    Image.asset(
                      Img.get('material_bg_1.png'),
                      width: double.infinity,
                      height: double.infinity,
                      fit: BoxFit.cover,
                    ),
                    Padding(
                      padding:
                          EdgeInsets.symmetric(vertical: 40, horizontal: 14),
                      child: CircleAvatar(
                        radius: 36,
                        backgroundColor: Colors.transparent,
                        child: CircleAvatar(
                          radius: 36,
                          backgroundColor: Colors.transparent,
                          backgroundImage:
                              AssetImage(Img.get("logo_small_round.png")),
                        ),
                      ),
                    ),
                    Align(
                      alignment: Alignment.bottomLeft,
                      child: Padding(
                        padding:
                            EdgeInsets.symmetric(horizontal: 20, vertical: 18),
                        child: Column(
                          mainAxisSize: MainAxisSize.min,
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: <Widget>[
                            Text(user.email,
                                style: MyText.body2(context).copyWith(
                                    color: Colors.grey[100],
                                    fontWeight: FontWeight.bold)),
                            Container(height: 5),
                            Text('',
                                style: MyText.body2(context)
                                    .copyWith(color: Colors.grey[100]))
                          ],
                        ),
                      ),
                    ),
                  ],
                ),
              ),
              ListTile(
                title: Text("Home",
                    style: MyText.subhead(context).copyWith(
                        color: Colors.black, fontWeight: FontWeight.w500)),
                leading: Icon(Icons.home, size: 25.0, color: Colors.grey),
                onTap: () {
                  Navigator.pop(context);
                },
              ),
              ListTile(
                title: Text("Compras",
                    style: MyText.subhead(context).copyWith(
                        color: Colors.black, fontWeight: FontWeight.w500)),
                leading: Icon(Icons.whatshot, size: 25.0, color: Colors.grey),
                onTap: () {
                  Navigator.of(context).pushNamed("/BrendaShopping");
                },
              ),
              Divider(),
              ListTile(
                title: Text("Configurações",
                    style: MyText.subhead(context).copyWith(
                        color: Colors.black, fontWeight: FontWeight.w500)),
                leading: Icon(Icons.settings, size: 25.0, color: Colors.grey),
                onTap: () {
                  Navigator.pop(context);
                },
              ),
              ListTile(
                title: Text("Ajuda",
                    style: MyText.subhead(context).copyWith(
                        color: Colors.black, fontWeight: FontWeight.w500)),
                leading:
                    Icon(Icons.help_outline, size: 25.0, color: Colors.grey),
                onTap: () {
                  Navigator.pop(context);
                },
              ),
              ListTile(
                title: Text("Sair",
                    style: MyText.subhead(context).copyWith(
                        color: Colors.black, fontWeight: FontWeight.w500)),
                leading:
                    Icon(Icons.exit_to_app, size: 25.0, color: Colors.grey),
                onTap: () => signOut(),
              ),
            ],
          ),
        ),
      ),
    );
  }

  void _showScanner() {
    sheetController = showBottomSheet(
        context: _scaffoldCtx,
        builder: (BuildContext bc) {
          return Card(
            elevation: 10,
            margin: EdgeInsets.fromLTRB(0, 0, 0, 0),
            child: Container(
                padding: EdgeInsets.all(10),
                width: double.infinity,
                color: Colors.white,
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: <Widget>[
                    Center(
                      child: Container(
                          width: 30,
                          height: 5,
                          decoration: BoxDecoration(
                            color: MyColors.grey_10,
                            borderRadius: BorderRadius.all(Radius.circular(5)),
                          )),
                    ),
                    Container(height: 10),
                    SizedBox(
                      height: 200,
                      child: QrCamera(
                        formats: [BarcodeFormats.ALL_FORMATS],
                        qrCodeCallback: handleScan,
                      ),
                    ),
                  ],
                )),
          );
        });
    sheetController.closed.then((value) {
      setState(() {
        showSheet = false;
      });
    });
  }

  void signOut() {
    _storage.deleteAll().then(
        (value) => Navigator.pushReplacementNamed(context, '/BrendaSignIn'));
  }

  Future<String> getData() async => _storage.read(key: 'user_username');

  Future _getCurrentPosition() async {
    final Geolocator geolocator = Geolocator()..forceAndroidLocationManager;
    try {
      Position pos = await geolocator
          .getCurrentPosition(desiredAccuracy: LocationAccuracy.best)
          .timeout(new Duration(seconds: 5));

      setState(() {
        gps.latitude = pos.latitude;
        gps.longitude = pos.longitude;
        gps.accuracy = pos.accuracy;
        gps.altitude = pos.altitude;
        gps.speed = pos.speed;
        gps.speedAccuracy = pos.speedAccuracy;
        gps.heading = pos.heading;
      });
    } catch (e) {
      //Toast.show("Erro de GeoLocator " + e.toString(), context);
    }
  }

  Future handleScan(String code) async {
    if (canScan) {
      setState(() {
        canScan = false;
        Navigator.pop(_scaffoldCtx);
      });

      final regexp = RegExp(r"http:////uau.tw\/[A-F0-9]{8,8}$");
      if (!regexp.hasMatch(code)) {
        Toast.show("QRCode inválido!", context);
      } else {
        var tag = code.replaceAll(new RegExp(r"http:////uau.tw\/"), '');

        Navigator.push(
            context,
            MaterialPageRoute(
                builder: (context) => BrendaActionRoute(
                    gps.withTag(tag), _deviceId, _deviceData)));
      }
    }
  }

  Widget appBar() {
    return AppBar(
        elevation: 0,
        backgroundColor: Colors.transparent,
        leading: IconButton(
          icon: Icon(Icons.menu, color: Colors.white),
          onPressed: () {
            scaffoldKey.currentState.openDrawer();
          },
        ),
        actions: <Widget>[
          IconButton(
            icon: Icon(Icons.refresh, color: Colors.white),
            onPressed: refresh,
          ),
        ]);
  }

  Widget balance(User user) {
    return Card(
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(6)),
      color: Colors.white,
      elevation: 2,
      clipBehavior: Clip.antiAliasWithSaveLayer,
      child: Column(
        children: <Widget>[
          Row(
            children: <Widget>[
              Container(width: 10),
              Expanded(
                child: Text("Carteira Brenda",
                    style: MyText.subhead(context)
                        .copyWith(color: MyColors.grey_40)),
              ),
              IconButton(
                  icon: Icon(Icons.add, color: MyColors.grey_40),
                  onPressed: () {}),
            ],
          ),
          Container(height: 10),
          Text("Saldo BRE\$",
              style:
                  MyText.subhead(context).copyWith(color: Colors.green[300])),
          Text(_currency.format(_balance),
              style: MyText.display1(context).copyWith(color: Colors.black)),
          Text("R\$ 0,00",
              style: MyText.subhead(context).copyWith(color: MyColors.grey_40)),
          Container(height: 25),
        ],
      ),
    );
  }

  Widget sendReceive() {
    return Row(
      children: <Widget>[
        Expanded(
          child: GestureDetector(
            onTap: () {
              Navigator.of(context).pushNamed('/BrendaSend');
            },
            child: Card(
              shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(6)),
              color: Colors.white,
              elevation: 2,
              clipBehavior: Clip.antiAliasWithSaveLayer,
              child: Container(
                padding: EdgeInsets.all(15),
                child: Row(
                  children: <Widget>[
                    CircleAvatar(
                      radius: 12,
                      backgroundColor: MyColors.grey_10,
                      child: Icon(Icons.arrow_upward,
                          color: MyColors.grey_40, size: 15),
                    ),
                    Container(width: 15),
                    Text("Enviar",
                        style: MyText.subhead(context).copyWith(
                            color: Colors.green[900],
                            fontWeight: FontWeight.w500))
                  ],
                ),
              ),
            ),
          ),
        ),
        Container(width: 5),
        Expanded(
          child: GestureDetector(
            onTap: () {
              Navigator.of(context)
                  .pushNamed('/BrendaReceive', arguments: _userId);
            },
            child: Card(
              shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(6)),
              color: Colors.white,
              elevation: 2,
              clipBehavior: Clip.antiAliasWithSaveLayer,
              child: Container(
                padding: EdgeInsets.all(15),
                child: Row(
                  children: <Widget>[
                    CircleAvatar(
                      radius: 12,
                      backgroundColor: MyColors.grey_10,
                      child: Icon(Icons.arrow_downward,
                          color: MyColors.grey_40, size: 15),
                    ),
                    Container(width: 15),
                    Text("Receber",
                        style: MyText.subhead(context).copyWith(
                            color: Colors.green[900],
                            fontWeight: FontWeight.w500))
                  ],
                ),
              ),
            ),
          ),
        ),
      ],
    );
  }

  Future<void> initPlatformState() async {
    Map<String, dynamic> deviceData;

    try {
      if (Platform.isAndroid) {
        deviceData = _readAndroidBuildData(await deviceInfoPlugin.androidInfo);
      } else if (Platform.isIOS) {
        deviceData = _readIosDeviceInfo(await deviceInfoPlugin.iosInfo);
      }
    } on PlatformException {
      deviceData = <String, dynamic>{
        'Error:': 'Failed to get platform version.'
      };
    }

    if (!mounted) return;

    setState(() {
      _deviceId = deviceData['androidId'];
      _deviceData = json.encode(deviceData);
    });
  }

  Map<String, dynamic> _readAndroidBuildData(AndroidDeviceInfo build) {
    return <String, dynamic>{
      'version.securityPatch': build.version.securityPatch,
      'version.sdkInt': build.version.sdkInt,
      'version.release': build.version.release,
      'version.previewSdkInt': build.version.previewSdkInt,
      'version.incremental': build.version.incremental,
      'version.codename': build.version.codename,
      'version.baseOS': build.version.baseOS,
      'board': build.board,
      'bootloader': build.bootloader,
      'brand': build.brand,
      'device': build.device,
      'display': build.display,
      'fingerprint': build.fingerprint,
      'hardware': build.hardware,
      'host': build.host,
      'id': build.id,
      'manufacturer': build.manufacturer,
      'model': build.model,
      'product': build.product,
      'supported32BitAbis': build.supported32BitAbis,
      'supported64BitAbis': build.supported64BitAbis,
      'supportedAbis': build.supportedAbis,
      'tags': build.tags,
      'type': build.type,
      'isPhysicalDevice': build.isPhysicalDevice,
      'androidId': build.androidId,
      'systemFeatures': build.systemFeatures,
    };
  }

  Map<String, dynamic> _readIosDeviceInfo(IosDeviceInfo data) {
    return <String, dynamic>{
      'name': data.name,
      'systemName': data.systemName,
      'systemVersion': data.systemVersion,
      'model': data.model,
      'localizedModel': data.localizedModel,
      'identifierForVendor': data.identifierForVendor,
      'isPhysicalDevice': data.isPhysicalDevice,
      'utsname.sysname:': data.utsname.sysname,
      'utsname.nodename:': data.utsname.nodename,
      'utsname.release:': data.utsname.release,
      'utsname.version:': data.utsname.version,
      'utsname.machine:': data.utsname.machine,
    };
  }
}
