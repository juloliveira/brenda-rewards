import 'package:brenda_wallet/api/token_client.dart';
import 'package:brenda_wallet/database/database.dart';
import 'package:flutter/material.dart';
import 'package:brenda_wallet/data/img.dart';
import 'package:brenda_wallet/data/my_colors.dart';
import 'package:brenda_wallet/widget/my_text.dart';
import 'package:flutter_flavor/flutter_flavor.dart';
import 'package:toast/toast.dart';

class BrendaSignInRoute extends StatefulWidget {
  BrendaSignInRoute();

  @override
  BrendaSignInRouteState createState() => new BrendaSignInRouteState();
}

class BrendaSignInRouteState extends State<BrendaSignInRoute> {
  final TextEditingController _email =
      new TextEditingController(text: FlavorConfig.instance.variables["user"]);

  final TextEditingController _pass =
      new TextEditingController(text: FlavorConfig.instance.variables["pass"]);

  @override
  Widget build(BuildContext context) {
    return new Scaffold(
      resizeToAvoidBottomInset: false,
      backgroundColor: Colors.white,
      appBar:
          PreferredSize(child: Container(), preferredSize: Size.fromHeight(0)),
      body: Container(
        padding: EdgeInsets.symmetric(vertical: 30, horizontal: 30),
        width: double.infinity,
        height: double.infinity,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            Container(height: 30),
            Container(
              child: Image.asset(Img.get('logo_small.png')),
              width: 80,
              height: 80,
            ),
            Container(height: 15),
            Text("Bem-vindo,",
                style: MyText.title(context).copyWith(
                    color: MyColors.grey_80, fontWeight: FontWeight.bold)),
            Container(height: 5),
            Text("Faça login para continuar",
                style: MyText.subhead(context).copyWith(
                    color: Colors.blueGrey[300], fontWeight: FontWeight.bold)),
            TextField(
              keyboardType: TextInputType.emailAddress,
              controller: _email,
              decoration: InputDecoration(labelText: "E-mail"),
            ),
            Container(height: 25),
            TextField(
              keyboardType: TextInputType.visiblePassword,
              obscureText: true,
              controller: _pass,
              decoration: InputDecoration(labelText: "Senha"),
            ),
            Container(height: 5),
            Row(
              children: <Widget>[
                Spacer(),
                FlatButton(
                    child: Text(
                      "Esqueceu sua senha?",
                      style: TextStyle(color: Colors.blueAccent[400]),
                    ),
                    color: Colors.transparent,
                    onPressed: () =>
                        Navigator.of(context).pushNamed('/BrendaPassword')),
              ],
            ),
            Container(
              width: double.infinity,
              child: FlatButton(
                child: Text(
                  "Login",
                  style: TextStyle(color: Colors.white),
                ),
                color: Colors.blueAccent[400],
                onPressed: _login,
              ),
            ),
            Container(
              width: double.infinity,
              child: FlatButton(
                child: Text(
                  "Novo por aqui? Crie sua conta!",
                  style: TextStyle(color: Colors.blueAccent[400]),
                ),
                color: Colors.transparent,
                onPressed: () {
                  Navigator.of(context).pushNamed("/BrendaSignUpWizard");
                },
              ),
            )
          ],
          mainAxisSize: MainAxisSize.min,
        ),
      ),
    );
  }

  void _login() async {
    TokenClient()
        .signIn(_email.text, _pass.text)
        .then((token) async => await Db().saveToken(_email.text, token))
        .then((signin) =>
            Navigator.pushReplacementNamed(context, '/BrendaWallet'))
        .catchError((ex) => {
              Toast.show(
                  "Ocorreu um erro ao efetuar login. ${ex.toString()}", context,
                  duration: 10)
            });
  }
}
