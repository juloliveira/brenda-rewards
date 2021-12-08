import 'package:flutter/material.dart';

class ListForm extends StatefulWidget {
  @override
  ListFormState createState() => new ListFormState();
}

class ListFormState extends State<ListForm> {
  List<String> products = ["Test1", "Test2", "Test3"];
  @override
  Widget build(BuildContext context) {
    return new Container(
        child: new Center(
            child: new Row(children: <Widget>[
      new Row(
        children: <Widget>[
          new ListView.builder(
              itemCount: products.length,
              itemBuilder: (BuildContext ctxt, int index) {
                return new Text(products[index]);
              }),
          new IconButton(
            icon: Icon(Icons.remove_circle),
            onPressed: () {},
          )
        ],
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
      ),
      new TextField(
        decoration: new InputDecoration(hintText: "Prodotto"),
        onSubmitted: (String str) {
          setState(() {
            products.add(str);
          });
        },
      ),
    ])));
  }
}
