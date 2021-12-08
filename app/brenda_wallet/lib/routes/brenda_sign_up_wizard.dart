import 'dart:async';
import 'dart:convert';
import 'dart:io';

import 'package:brenda_wallet/api/user_client.dart';
import 'package:brenda_wallet/models/brenda_gps.dart';
import 'package:brenda_wallet/models/sex.dart';
import 'package:brenda_wallet/models/user_register.dart';
import 'package:device_info/device_info.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:brenda_wallet/data/img.dart';
import 'package:brenda_wallet/data/my_colors.dart';
import 'package:brenda_wallet/widget/my_text.dart';
import 'package:flutter/services.dart';
import 'package:g_captcha/g_captcha.dart';
import 'package:geolocator/geolocator.dart';
import 'package:toast/toast.dart';
import 'package:flutter_masked_text/flutter_masked_text.dart';

class BrendaSignUpWizardRoute extends StatefulWidget {
  BrendaSignUpWizardRoute();

  @override
  BrendaSignUpWizardRouteState createState() =>
      new BrendaSignUpWizardRouteState();
}

class BrendaSignUpWizardRouteState extends State<BrendaSignUpWizardRoute> {
  static const EMAIL_REGEXP =
      r"^[a-zA-Z0-9.a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9]+\.[a-zA-Z]+";

  static final DeviceInfoPlugin deviceInfoPlugin = DeviceInfoPlugin();
  int _sex = Sex.male;
  String _genre = Gender.birthgender;
  String _sexuality = Sexuality.heterosexual;
  String _income = Income.classA;
  String _education = Education.level1;
  BrendaGps gps = BrendaGps();
  Timer _timerPosition;
  String _deviceId;
  String _deviceData;

  static const CAPTCHA_SITE_KEY = "6Ld6qr0ZAAAAAN4HIcpKeWwxsEdDlviYDKsqLp33";

  final TextEditingController email =
      new TextEditingController(); //(text: "jul.oliveira@gmail.com");
  final TextEditingController phone = new MaskedTextController(
      mask:
          '(00) 00000-0000'); //(mask: '(00) 00000-0000', text: "(11) 96649-6848");
  final TextEditingController birthdate = new MaskedTextController(
      mask: '00/00/0000'); //(mask: '00/00/0000', text: "24/12/1980");
  final TextEditingController document = new MaskedTextController(
      mask:
          '000.000.000-00'); //(mask: '000.000.000-00', text: "280.648.668-80");

  final _form = GlobalKey<FormState>();

  PageController pageController = PageController(
    initialPage: 0,
  );
  int page = 0;
  bool isLast = false;

  @override
  void initState() {
    super.initState();

    initPlatformState();
    _getCurrentPosition();
    _timerPosition = Timer.periodic(Duration(seconds: 7), (timer) {
      _getCurrentPosition();
    });
  }

  @override
  void dispose() {
    print("!=========================================> DISPOSE SIGNUP ROUTE!");
    _timerPosition.cancel();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return new Scaffold(
      backgroundColor: Colors.grey[100],
      appBar: AppBar(
        title: Text("Cadastro Brenda"),
      ),
      body: Container(
        width: double.infinity,
        height: double.infinity,
        child: Column(children: <Widget>[
          Expanded(
            child: PageView(
              onPageChanged: onPageViewChange,
              controller: pageController,
              //children: buildPageViewItem(),
              children: [
                preStep(context),
                stepGender(context),
                stepRent(context),
                stepEducationLevel(context),
                stepEmailPass(context),
              ],
            ),
          ),
          Container(height: 15),
          Align(
            alignment: Alignment.topCenter,
            child: Container(
              height: 20,
              child: Align(
                alignment: Alignment.topCenter,
                child: buildDots(context),
              ),
            ),
          )
        ]),
      ),
    );
  }

  void onPageViewChange(int _page) {
    page = _page;
    isLast = _page == 4;
    setState(() {});
  }

  Widget buildDots(BuildContext context) {
    Widget widget;

    List<Widget> dots = [];
    for (int i = 0; i < 5; i++) {
      Widget w = Container(
        margin: EdgeInsets.symmetric(horizontal: 5),
        height: 8,
        width: 8,
        child: CircleAvatar(
          backgroundColor: page == i ? Colors.green[600] : MyColors.grey_20,
        ),
      );
      dots.add(w);
    }
    widget = Row(
      mainAxisSize: MainAxisSize.min,
      children: dots,
    );
    return widget;
  }

  Widget preStep(BuildContext context) {
    return Container(
        padding: EdgeInsets.all(20),
        width: 280,
        height: 370,
        child: Card(
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(3)),
          clipBehavior: Clip.antiAliasWithSaveLayer,
          elevation: 2,
          child: Stack(
            children: <Widget>[
              Container(color: Colors.blueAccent[400].withOpacity(0.7)),
              Column(
                children: <Widget>[
                  Container(height: 35),
                  Text("Cadastrar Usuário",
                      style: MyText.title(context).copyWith(
                          color: Colors.white, fontWeight: FontWeight.bold)),
                  Container(
                    margin: EdgeInsets.symmetric(vertical: 10, horizontal: 25),
                    child: Text(
                        "Na Brenda respeitamos a diversidade. " +
                            "As questões a seguir são para que possamos conhecer " +
                            "você melhor e proporcionar a você a melhor " +
                            "experiência respeitando sua individualidade. ",
                        textAlign: TextAlign.center,
                        style: MyText.subhead(context)
                            .copyWith(color: Colors.white)),
                  ),
                  Expanded(
                    child: Image.asset(Img.get('logo_small.png'), width: 150),
                  ),
                  Container(
                    width: double.infinity,
                    height: 50,
                    child: FlatButton(
                      child: Text("Começar Cadastro",
                          style: MyText.subhead(context)
                              .copyWith(color: Colors.white)),
                      color: Colors.blueAccent[400],
                      onPressed: () {
                        if (isLast) {
                          Navigator.pop(context);
                          return;
                        }
                        pageController.nextPage(
                            duration: Duration(milliseconds: 300),
                            curve: Curves.easeOut);
                      },
                    ),
                  ),
                ],
              )
            ],
          ),
        ));
  }

  Widget stepGender(BuildContext context) {
    return SingleChildScrollView(
      child: Container(
        padding: EdgeInsets.all(20),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            Text("Qual é seu gênero de nascimento?",
                style: MyText.body2(context).copyWith(color: MyColors.grey_60)),
            Container(height: 5),
            Column(
              children: [
                ListTile(
                    title: const Text('Eu nasci um MENINO'),
                    leading: Radio(
                      value: Sex.male,
                      groupValue: _sex,
                      onChanged: (int sex) {
                        setState(() {
                          _sex = sex;
                        });
                      },
                    )),
                ListTile(
                    title: const Text('Eu nasci uma MENINA'),
                    leading: Radio(
                      value: Sex.female,
                      groupValue: _sex,
                      onChanged: (int sex) {
                        setState(() {
                          _sex = sex;
                        });
                      },
                    )),
              ],
            ),
            Container(height: 15),
            Text("E como você define sua identidade de gênero?",
                style: MyText.body2(context).copyWith(color: MyColors.grey_60)),
            Container(height: 15),
            Column(
              children: [
                ListTile(
                    title: const Text(
                        'Me identifico pelo meu \r\nGÊNERO DE NASCIMENTO'),
                    leading: Radio(
                      value: Gender.birthgender,
                      groupValue: _genre,
                      onChanged: (String value) {
                        setState(() {
                          _genre = value;
                        });
                      },
                    )),
                ListTile(
                    title: const Text('Sua uma pessoa Transgênero'),
                    leading: Radio(
                      value: Gender.transgender,
                      groupValue: _genre,
                      onChanged: (String value) {
                        setState(() {
                          _genre = value;
                        });
                      },
                    )),
                ListTile(
                    title: const Text('Sou Andrógeno'),
                    leading: Radio(
                      value: Gender.androgen,
                      groupValue: _genre,
                      onChanged: (String value) {
                        setState(() {
                          _genre = value;
                        });
                      },
                    )),
                ListTile(
                    title: const Text('Me identifico como Não Binário'),
                    leading: Radio(
                      value: Gender.nonbinary,
                      groupValue: _genre,
                      onChanged: (String value) {
                        setState(() {
                          _genre = value;
                        });
                      },
                    )),
                ListTile(
                    title: const Text('Gênero Flutuante'),
                    leading: Radio(
                      value: Gender.floatinggenre,
                      groupValue: _genre,
                      onChanged: (String value) {
                        setState(() {
                          _genre = value;
                        });
                      },
                    )),
              ],
            ),
            Container(height: 15),
            Text("E como você se define sexualmente?",
                style: MyText.body2(context).copyWith(color: MyColors.grey_60)),
            Container(height: 15),
            Column(
              children: [
                ListTile(
                    title: const Text('Heterosexual'),
                    leading: Radio(
                      value: Sexuality.heterosexual,
                      groupValue: _sexuality,
                      onChanged: (String value) {
                        setState(() {
                          _sexuality = value;
                        });
                      },
                    )),
                ListTile(
                    title: const Text('Homosexual (Gay ou lésbica)'),
                    leading: Radio(
                      value: Sexuality.homosexual,
                      groupValue: _sexuality,
                      onChanged: (String value) {
                        setState(() {
                          _sexuality = value;
                        });
                      },
                    )),
                ListTile(
                    title: const Text('Bissexual'),
                    leading: Radio(
                      value: Sexuality.bissexual,
                      groupValue: _sexuality,
                      onChanged: (String value) {
                        setState(() {
                          _sexuality = value;
                        });
                      },
                    )),
                ListTile(
                    title: const Text('Pansexual'),
                    leading: Radio(
                      value: Sexuality.pansexual,
                      groupValue: _sexuality,
                      onChanged: (String value) {
                        setState(() {
                          _sexuality = value;
                        });
                      },
                    )),
                ListTile(
                    title: const Text('Demisexual'),
                    leading: Radio(
                      value: Sexuality.demisexual,
                      groupValue: _sexuality,
                      onChanged: (String value) {
                        setState(() {
                          _sexuality = value;
                        });
                      },
                    )),
                ListTile(
                    title: const Text('Assexual'),
                    leading: Radio(
                      value: Sexuality.assexual,
                      groupValue: _sexuality,
                      onChanged: (String value) {
                        setState(() {
                          _sexuality = value;
                        });
                      },
                    )),
              ],
            ),
            Container(height: 15),
            Container(
              width: double.infinity,
              height: 50,
              child: FlatButton(
                child: Text(isLast ? "Criar meu usuário" : "Próximo Passo",
                    style:
                        MyText.subhead(context).copyWith(color: Colors.white)),
                color: Colors.blueAccent[400],
                onPressed: () {
                  if (isLast) {
                    Navigator.pop(context);
                    return;
                  }
                  pageController.nextPage(
                      duration: Duration(milliseconds: 300),
                      curve: Curves.easeOut);
                },
              ),
            )
          ],
        ),
      ),
    );
  }

  Widget stepRent(BuildContext context) {
    return SingleChildScrollView(
      child: Container(
        padding: EdgeInsets.all(20),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            Text("Qual sua renda familiar?",
                style: MyText.body1(context).copyWith(color: MyColors.grey_60)),
            Container(height: 5),
            Column(
              children: [
                ListTile(
                    title: const Text("Minha renda é de até R\$ 1.045"),
                    leading: Radio(
                      value: Income.classA,
                      groupValue: _income,
                      onChanged: (String value) {
                        setState(() {
                          _income = value;
                        });
                      },
                    )),
                ListTile(
                    title: const Text('Entre R\$ 1.045 até R\$ 5.000'),
                    leading: Radio(
                      value: Income.classB,
                      groupValue: _income,
                      onChanged: (String value) {
                        setState(() {
                          _income = value;
                        });
                      },
                    )),
                ListTile(
                    title: const Text('Entre R\$ 5.000 até R\$ 10.000'),
                    leading: Radio(
                      value: Income.classC,
                      groupValue: _income,
                      onChanged: (String value) {
                        setState(() {
                          _income = value;
                        });
                      },
                    )),
                ListTile(
                    title: const Text('Mais de R\$ 10.000'),
                    leading: Radio(
                      value: Income.classD,
                      groupValue: _income,
                      onChanged: (String value) {
                        setState(() {
                          _income = value;
                        });
                      },
                    )),
              ],
            ),
            Container(height: 15),
            Container(
              width: double.infinity,
              height: 50,
              child: FlatButton(
                child: Text(isLast ? "Criar meu usuário" : "Próximo Passo",
                    style:
                        MyText.subhead(context).copyWith(color: Colors.white)),
                color: Colors.blueAccent[400],
                onPressed: () {
                  if (isLast) {
                    Navigator.pop(context);
                    return;
                  }
                  pageController.nextPage(
                      duration: Duration(milliseconds: 300),
                      curve: Curves.easeOut);
                },
              ),
            )
          ],
        ),
      ),
    );
  }

  Widget stepEducationLevel(BuildContext context) {
    return SingleChildScrollView(
      child: Container(
        padding: EdgeInsets.all(20),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            Text(
                "Qual sua escolaridade? (Que você tenha terminado ou está cursando)",
                style: MyText.body1(context).copyWith(color: MyColors.grey_60)),
            Container(height: 5),
            Column(
              children: [
                ListTile(
                    title: const Text("Ensino Fundamental (Primário)"),
                    leading: Radio(
                      value: Education.level1,
                      groupValue: _education,
                      onChanged: (String value) {
                        setState(() {
                          _education = value;
                        });
                      },
                    )),
                ListTile(
                    title: const Text('Ensino Médio (Ginário)'),
                    leading: Radio(
                      value: Education.level2,
                      groupValue: _education,
                      onChanged: (String value) {
                        setState(() {
                          _education = value;
                        });
                      },
                    )),
                ListTile(
                    title: const Text('Ensino Superior (Faculdade)'),
                    leading: Radio(
                      value: Education.level3,
                      groupValue: _education,
                      onChanged: (String value) {
                        setState(() {
                          _education = value;
                        });
                      },
                    )),
                ListTile(
                    title: const Text('Pós-graduação'),
                    leading: Radio(
                      value: Education.level4,
                      groupValue: _education,
                      onChanged: (String value) {
                        setState(() {
                          _education = value;
                        });
                      },
                    )),
              ],
            ),
            Container(height: 15),
            Container(
              width: double.infinity,
              height: 50,
              child: FlatButton(
                child: Text(isLast ? "Criar meu usuário" : "Próximo Passo",
                    style:
                        MyText.subhead(context).copyWith(color: Colors.white)),
                color: Colors.blueAccent[400],
                onPressed: () {
                  if (isLast) {
                    Navigator.pop(context);
                    return;
                  }
                  pageController.nextPage(
                      duration: Duration(milliseconds: 300),
                      curve: Curves.easeOut);
                },
              ),
            )
          ],
        ),
      ),
    );
  }

  Widget stepEmailPass(BuildContext context) {
    return SingleChildScrollView(
      child: Container(
        padding: EdgeInsets.all(20),
        child: Form(
          key: _form,
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: <Widget>[
              Text("Digite seu e-mail:",
                  style:
                      MyText.body1(context).copyWith(color: MyColors.grey_60)),
              Container(height: 5),
              Card(
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(3),
                ),
                clipBehavior: Clip.antiAliasWithSaveLayer,
                margin: EdgeInsets.all(0),
                elevation: 0,
                child: Container(
                  height: 40,
                  alignment: Alignment.centerLeft,
                  padding: EdgeInsets.symmetric(horizontal: 20),
                  child: TextFormField(
                    maxLines: 1,
                    keyboardType: TextInputType.emailAddress,
                    controller: email,
                    validator: (value) {
                      if (!RegExp(EMAIL_REGEXP).hasMatch(value))
                        return "Você não digitou um email válido.";
                      return null;
                    },
                    decoration: InputDecoration(
                      contentPadding: EdgeInsets.all(-12),
                      border: InputBorder.none,
                    ),
                  ),
                ),
              ),
              Container(height: 15),
              Text("Seu CPF:",
                  style:
                      MyText.body1(context).copyWith(color: MyColors.grey_60)),
              Container(height: 5),
              Card(
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(3),
                ),
                clipBehavior: Clip.antiAliasWithSaveLayer,
                margin: EdgeInsets.all(0),
                elevation: 0,
                child: Container(
                  height: 40,
                  alignment: Alignment.centerLeft,
                  padding: EdgeInsets.symmetric(horizontal: 20),
                  child: TextFormField(
                    maxLines: 1,
                    keyboardType: TextInputType.number,
                    controller: document,
                    validator: (value) {
                      if (value.length != 14)
                        return "Você não digitou um CPF válido.";
                      return null;
                    },
                    decoration: InputDecoration(
                      contentPadding: EdgeInsets.all(-12),
                      border: InputBorder.none,
                    ),
                  ),
                ),
              ),
              Container(height: 15),
              Text("Número do seu Telefone Celular:",
                  style:
                      MyText.body1(context).copyWith(color: MyColors.grey_60)),
              Container(height: 5),
              Card(
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(3),
                ),
                clipBehavior: Clip.antiAliasWithSaveLayer,
                margin: EdgeInsets.all(0),
                elevation: 0,
                child: Container(
                  height: 40,
                  alignment: Alignment.centerLeft,
                  padding: EdgeInsets.symmetric(horizontal: 20),
                  child: TextFormField(
                    maxLines: 1,
                    controller: phone,
                    keyboardType: TextInputType.number,
                    decoration: InputDecoration(
                      contentPadding: EdgeInsets.all(-12),
                      border: InputBorder.none,
                    ),
                  ),
                ),
              ),
              Container(height: 15),
              Text("Data de Nascimento:",
                  style:
                      MyText.body1(context).copyWith(color: MyColors.grey_60)),
              Container(height: 5),
              Card(
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(3),
                ),
                clipBehavior: Clip.antiAliasWithSaveLayer,
                margin: EdgeInsets.all(0),
                elevation: 0,
                child: Container(
                  height: 40,
                  alignment: Alignment.centerLeft,
                  padding: EdgeInsets.symmetric(horizontal: 20),
                  child: TextFormField(
                      maxLines: 1,
                      controller: birthdate,
                      keyboardType: TextInputType.number,
                      decoration: InputDecoration(
                        contentPadding: EdgeInsets.all(-12),
                        border: InputBorder.none,
                      )),
                ),
              ),
              Container(height: 15),
              Container(height: 35),
              Text(
                  "Nós criaremos sua senha e será enviada para você em seu email. Você poderá alterar sua senha posteriormente."),
              Container(height: 15),
              Container(
                width: double.infinity,
                height: 50,
                child: FlatButton(
                  child: Text(isLast ? "Criar meu usuário" : "Próximo Passo",
                      style: MyText.subhead(context)
                          .copyWith(color: Colors.white)),
                  color: Colors.blueAccent[400],
                  onPressed: () {
                    if (isLast) {
                      createUser(context);
                      //Navigator.pop(context);
                      return;
                    }
                    pageController.nextPage(
                        duration: Duration(milliseconds: 300),
                        curve: Curves.easeOut);
                  },
                ),
              )
            ],
          ),
        ),
      ),
    );
  }

  Widget buildLoading(BuildContext context) {
    return Scaffold(
      body: Align(
        child: Container(
          width: 205,
          alignment: Alignment.center,
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: <Widget>[
              Container(
                width: 60,
                height: 60,
                child: Image.asset(Img.get('logo_small_round.png'),
                    fit: BoxFit.cover),
              ),
              Text("Por favor, aguarde...",
                  style: MyText.subhead(context)
                      .copyWith(color: Colors.grey[800])),
              Container(height: 20),
              Container(
                height: 4,
                child: LinearProgressIndicator(
                  backgroundColor: Colors.grey[300],
                ),
              ),
            ],
          ),
        ),
        alignment: Alignment.center,
      ),
    );
  }

  Future _getCurrentPosition() async {
    final Geolocator geolocator = Geolocator()..forceAndroidLocationManager;

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
  }

  void createUser(BuildContext context) async {
    if (_form.currentState.validate()) {
      String tokenResult =
          await GCaptcha.reCaptcha("6Lc_rb0ZAAAAAGU4T0NssbKVi1wEe3oFe6KdTUiM");
      if (tokenResult.isEmpty) {
        return null;
      }

      Navigator.push(context,
          MaterialPageRoute(builder: (context) => buildLoading(context)));

      await UserClient()
          .registerUser(UserRegister(
              email: email.text,
              document: document.text,
              birthdate: birthdate.text,
              phoneNumber: phone.text,
              sex: _sex,
              incomeId: _income,
              genderIdentityId: _genre,
              sexualityId: _sexuality,
              educationLevelId: _education,
              latitude: gps.latitude,
              longitude: gps.longitude,
              deviceId: _deviceId,
              deviceData: _deviceData,
              token: tokenResult))
          .then((value) {
        Navigator.of(context).pop();

        if (value['result']) {
          debugPrint("Cadastrado!");
          Navigator.of(context).popAndPushNamed('/BrendaWelcome');
        } else {
          Toast.show(value['message'], context,
              duration: 3, backgroundColor: Colors.redAccent[400]);
        }
      }).catchError((onError) {
        Navigator.of(context).pop();
        Toast.show("Seu formulário não está válido.", context,
            duration: 3, backgroundColor: Colors.redAccent[400]);
      });
    } else {
      Toast.show("Seu formulário não está válido.", context,
          duration: 3, backgroundColor: Colors.blueAccent[400]);
    }
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
