import 'package:brenda_wallet/api/user_client.dart';
import 'package:brenda_wallet/data/img.dart';
import 'package:brenda_wallet/data/my_colors.dart';
import 'package:brenda_wallet/widget/my_text.dart';
import 'package:flutter/material.dart';

enum PasswordState { form, requesting, finished }

class BrendaPasswordRoute extends StatefulWidget {
  BrendaPasswordRoute();

  @override
  BrendaPasswordRouteState createState() => new BrendaPasswordRouteState();
}

class BrendaPasswordRouteState extends State<BrendaPasswordRoute> {
  final TextEditingController _email =
      new TextEditingController(); //(text: "juliano@brendarewards.com");

  Future<PasswordState> _passwordFuture;

  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return new Scaffold(
        resizeToAvoidBottomInset: false,
        backgroundColor: Colors.white,
        appBar: PreferredSize(
            child: Container(), preferredSize: Size.fromHeight(0)),
        body: FutureBuilder<PasswordState>(
          initialData: PasswordState.form,
          future: _passwordFuture,
          builder: (context, snapshot) {
            switch (snapshot.data) {
              case PasswordState.form:
                return passwordForm();
              case PasswordState.finished:
                return finished();
              default:
                return Center(child: CircularProgressIndicator());
            }
          },
        ));
  }

  Widget passwordForm() {
    return Container(
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
          Text("Esqueceu sua senha?",
              style: MyText.title(context).copyWith(
                  color: MyColors.grey_80, fontWeight: FontWeight.bold)),
          Container(height: 5),
          Text("Vamos gerar uma nova senha para você!",
              style: MyText.subhead(context).copyWith(
                  color: Colors.blueGrey[300], fontWeight: FontWeight.bold)),
          Container(height: 25),
          TextField(
            keyboardType: TextInputType.emailAddress,
            controller: _email,
            decoration: InputDecoration(labelText: "E-mail"),
          ),
          Container(height: 25),
          Container(
            width: double.infinity,
            child: FlatButton(
              child: Text(
                "Enviar senha para meu E-mail",
                style: TextStyle(color: Colors.white),
              ),
              color: Colors.blueAccent[400],
              onPressed: () => createPassword(),
            ),
          ),
        ],
        mainAxisSize: MainAxisSize.min,
      ),
    );
  }

  Widget finished() {
    return Container(
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
          Text("Tudo pronto!",
              style: MyText.title(context).copyWith(
                  color: MyColors.grey_80, fontWeight: FontWeight.bold)),
          Container(height: 5),
          Text("Enviamos uma nova senha para você.",
              style: MyText.subhead(context).copyWith(
                  color: Colors.blueGrey[300], fontWeight: FontWeight.bold)),
          Text("Verifique seu E-mail!",
              style: MyText.subhead(context).copyWith(
                  color: Colors.blueGrey[300], fontWeight: FontWeight.bold)),
          Container(height: 25),
          Container(
            width: double.infinity,
            child: FlatButton(
              child: Text(
                "Voltar para Login",
                style: TextStyle(color: Colors.white),
              ),
              color: Colors.blueAccent[400],
              onPressed: () => Navigator.pop(context),
            ),
          ),
        ],
        mainAxisSize: MainAxisSize.min,
      ),
    );
  }

  void createPassword() {
    setState(() {
      _passwordFuture = Future<PasswordState>(() => PasswordState.requesting);
    });

    UserClient().requestNewPassword(_email.text).then((value) {
      //throw new Exception();
      setState(() {
        _passwordFuture = Future<PasswordState>(() => PasswordState.finished);
      });
    }).catchError((onError) {
      setState(() {
        _passwordFuture = Future<PasswordState>(() => PasswordState.form);
      });
    });
  }
}
