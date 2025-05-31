window.confirmDelete = (message) => {
  return confirm(message);
};

window.scrollToBottom = () => {
  const el = document.getElementById("chatContainer");
  if (el) {
    el.scrollTop = el.scrollHeight;
  }
};

window.getScrollHeight = () => {
  const el = document.getElementById("chatContainer");
  if (!el) return 0;

  return el.scrollHeight;
};

window.getScrollPercentage = () => {
  const el = document.getElementById("chatContainer");
  if (!el) return 0;

  const scrollTop = el.scrollTop;
  const scrollHeight = el.scrollHeight - el.clientHeight;

  if (scrollHeight === 0) return 100;

  return Math.floor((scrollTop / scrollHeight) * 100);
};

window.updateScroll = (prevHeight) => {
  const el = document.getElementById("chatContainer");
  if (el) {
    const newHeight = el.scrollHeight - prevHeight;
    el.scrollTop += newHeight;
  }
};
