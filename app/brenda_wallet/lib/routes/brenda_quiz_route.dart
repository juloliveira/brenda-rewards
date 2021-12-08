import 'package:brenda_wallet/models/voucher.dart';
import 'package:brenda_wallet/routes/brenda_reward.dart';
import 'package:flutter/material.dart';

class BrendaQuizRoute extends StatefulWidget {
  final Voucher voucher;

  BrendaQuizRoute(this.voucher);

  @override
  BrendaQuizRouteState createState() => BrendaQuizRouteState();
}

class BrendaQuizRouteState extends State<BrendaQuizRoute> {
  int atualPage = 0;
  String atualSelected = '';
  String atualId = '';
  List<String> replies = [];
  var _pageController = PageController(initialPage: 0);

  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    List<Widget> questoes = [];
    for (var item in widget.voucher.campaign.quiz) {
      questoes.add(questaoWidget(item));
    }
    return SafeArea(
      child: Scaffold(
        body: Container(
          color: Color(0xffeeeeee),
          child: Stack(
            children: <Widget>[
              widget.voucher.logo != null
                  ? Image.network(
                      widget.voucher.logo + "?amanha",
                      width: MediaQuery.of(context).size.width,
                      height: MediaQuery.of(context).size.height / 2,
                      fit: BoxFit.cover,
                    )
                  : Container(
                      height: MediaQuery.of(context).size.height / 2,
                      color: Colors.blue,
                    ),
              IconButton(
                icon: Icon(
                  Icons.arrow_back,
                  color: Colors.white,
                ),
                onPressed: () {
                  Navigator.of(context).pop();
                },
              ),
              Container(
                  margin: EdgeInsets.only(top: 45),
                  child: PageView(
                    controller: _pageController,
                    scrollDirection: Axis.horizontal,
                    pageSnapping: true,
                    physics: NeverScrollableScrollPhysics(),
                    children: questoes,
                  ))
            ],
          ),
        ),
      ),
    );
  }

  Widget questaoWidget(Quiz quiz) {
    List<Widget> alternatias = [];

    if (quiz.id == atualId) {
      _pageController.animateToPage(atualPage,
          duration: Duration(milliseconds: 200), curve: Curves.easeIn);
    }

    for (var op in quiz.options) {
      alternatias.add(ListTile(
        title: Text(op.description),
        leading: Radio(
            value: op.id,
            groupValue: atualSelected,
            onChanged: (value) {
              setState(() {
                replies.add(value);
                atualPage++;
                atualId = quiz.id;
                atualSelected = value;
              });
            }),
      ));
    }

    if (atualPage == widget.voucher.campaign.quiz.length) {
      alternatias.add(Container(
        margin: EdgeInsets.only(top: 15),
        decoration: BoxDecoration(
          color: Colors.blue,
        ),
        child: FlatButton(
          onPressed: () {
            print(replies);
            widget.voucher.campaign.replies = replies;
            Navigator.of(context)
                .pushReplacement(MaterialPageRoute(builder: (context) {
              return BrendaRewardRoute(widget.voucher);
            }));
          },
          child: Text(
            'Enviar Respostas',
            style: TextStyle(color: Colors.white),
          ),
        ),
      ));
    }

    return Padding(
      padding: const EdgeInsets.all(16),
      child: Center(
        child: Container(
          width: double.infinity,
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            mainAxisAlignment: MainAxisAlignment.center,
            children: <Widget>[
              Text(
                quiz.description,
                style: TextStyle(
                    fontWeight: FontWeight.bold,
                    color: Colors.white,
                    fontSize: 26,
                    backgroundColor: Colors.black.withOpacity(0.3),
                    shadows: [
                      BoxShadow(
                        color: Colors.black,
                        //blurRadius: 12,
                        offset: Offset(2, 2),
                      )
                    ]),
              ),
              Container(
                margin: EdgeInsets.symmetric(vertical: 16),
                padding: EdgeInsets.all(16),
                decoration: BoxDecoration(
                  color: Colors.white,
                  boxShadow: [
                    BoxShadow(
                      color: Colors.grey.withOpacity(0.5),
                      blurRadius: 1,
                      offset: Offset(0, 1),
                    )
                  ],
                ),
                child: Column(
                  children: alternatias,
                ),
              )
            ],
          ),
        ),
      ),
    );
  }
}
