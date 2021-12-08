class Voucher {
  final String id;
  final Campaign campaign;
  final String logo;

  Voucher(Map<String, dynamic> json)
      : id = json['voucher_id'],
        logo = json['lo'],
        campaign = Campaign(json['campaign']);
}

class Campaign {
  final String id;
  final String title;
  final String action;
  final String brand;
  final String resource;
  final List<Quiz> quiz;
  List<String> replies;

  Campaign(Map<String, dynamic> json)
      : id = json['id'],
        title = json['ti'],
        action = json['ac'],
        brand = json['br'],
        resource = json['rs'],
        replies = [],
        quiz = Quiz.build(json['quiz']);
}

class Quiz {
  String id;
  String description;
  int order;
  List<Quiz> options;

  static List<Quiz> build(Iterable quiz) {
    if (quiz == null) return null;
    List<Quiz> list = List<Quiz>();
    for (var item in quiz) list.add(Quiz.question(item));
    return list;
  }

  static Quiz question(Map<String, dynamic> json) {
    Quiz question = buildQuiz(json);
    question.options = Quiz.buildOptions(json['op']);
    return question;
  }

  static Quiz buildQuiz(json) {
    Quiz quiz = Quiz();
    quiz.id = json['id'];
    quiz.description = json['ds'];
    quiz.order = json['or'];
    return quiz;
  }

  static List<Quiz> buildOptions(Iterable options) {
    List<Quiz> list = List<Quiz>();
    for (var item in options) list.add(Quiz.buildQuiz(item));
    return list;
  }
}
