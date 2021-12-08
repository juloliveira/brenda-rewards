import 'package:property_change_notifier/property_change_notifier.dart';

class ShareIn extends PropertyChangeNotifier<String> {
  String _uri;

  String get uri => _uri;

  set uri(String value) {
    if (value == null) {
      _uri = null;
    } else {
      _uri = value;
      notifyListeners('uri');
    }
  }
}
