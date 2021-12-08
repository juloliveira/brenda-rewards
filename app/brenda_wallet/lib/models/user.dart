class User {
  final String id;
  final String email;
  final double balance;

  List<Transaction> transactions;

  User(this.id, this.balance, this.email);
}

class Transaction {
  String title;
  DateTime createdAt;
  String description;
  String customer;
  double value;
  int op;

  Transaction(
      {this.title,
      this.createdAt,
      this.description,
      this.customer,
      this.value,
      this.op});

  factory Transaction.fromJson(item) => Transaction(
      title: item['title'],
      description: item['description'],
      customer: item['customer'],
      op: int.parse(item['operation'].toString()),
      value: double.parse(item['value'].toString()),
      createdAt: DateTime.fromMillisecondsSinceEpoch(
          int.parse(item['created_at'].toString()) * 1000,
          isUtc: true));
}
