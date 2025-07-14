mergeInto(LibraryManager.library, {
  SendEvent: function (eventName, message) {
    var eventNameStr = UTF8ToString(eventName);
    var messageStr = UTF8ToString(message);

    var detail;
    try {
      detail = JSON.parse(messageStr);
    } catch (e) {
      detail = messageStr;
    }

    var customEvent = new CustomEvent(eventNameStr, { detail: detail });
    window.dispatchEvent(customEvent);
  },
});