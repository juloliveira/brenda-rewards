import 'package:flutter/material.dart';
import 'package:brenda_wallet/adapter/grid_shop_product_adapter.dart';
import 'package:brenda_wallet/data/dummy.dart';
import 'package:brenda_wallet/data/my_colors.dart';
import 'package:brenda_wallet/models/shop_category.dart';
import 'package:brenda_wallet/models/shop_product.dart';
import 'package:toast/toast.dart';

class BrendaShoppingRoute extends StatefulWidget {
  BrendaShoppingRoute();

  @override
  BrendaShoppingRouteState createState() => new BrendaShoppingRouteState();
}

class BrendaShoppingRouteState extends State<BrendaShoppingRoute> {
  BuildContext context;
  void onItemClick(int index, ShopCategory obj) {
    Toast.show("Produto " + index.toString() + "clicked", context,
        duration: Toast.LENGTH_SHORT);
  }

  @override
  Widget build(BuildContext context) {
    this.context = context;
    List<ShopProduct> items = Dummy.getShoppingProduct();
    return Scaffold(
      backgroundColor: MyColors.grey_10,
      appBar: AppBar(
          brightness: Brightness.dark,
          backgroundColor: MyColors.primary,
          title: Text("Loja Brenda", style: TextStyle(color: MyColors.grey_10)),
          leading: IconButton(
            icon: Icon(Icons.menu, color: MyColors.grey_10),
            onPressed: () {
              Navigator.pop(context);
            },
          ),
          actions: <Widget>[
            IconButton(
              icon: Icon(Icons.shopping_cart, color: MyColors.grey_10),
              onPressed: () {},
            ),
            IconButton(
              icon: Icon(Icons.more_vert, color: MyColors.grey_10),
              onPressed: () {},
            ), // overflow menu
          ]),
      body: GridShopProductAdapter(items, onItemClick).getView(),
    );
  }
}
