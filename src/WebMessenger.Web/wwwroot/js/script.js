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

window.getScrollValue = () => {
  const el = document.getElementById("chatContainer");
  if (!el)
    return 0;
  
  return el.scrollTop;
};

window.setScrollValue = (value) => {
  const el = document.getElementById("chatContainer");
  if (el){
    el.scrollTop = value;
    console.log(value)
  }
};

window.getScrollPercentage = () => {
  const el = document.getElementById("chatContainer");
  if (!el) return 0;

  const scrollTop = el.scrollTop;
  const scrollHeight = el.scrollHeight - el.clientHeight;

  if (scrollHeight === 0) return 100;

  return Math.floor((scrollTop / scrollHeight) * 100);
};

window.saveScroll = (prevHeight) => {
  const el = document.getElementById("chatContainer");
  if (el) {
    const newHeight = el.scrollHeight - prevHeight;
    el.scrollTop += newHeight;
  }
};

window.setTheme = (theme) => {
  const body = document.body;
  
  if (theme === 'light') {
    body.classList.remove('dark-mode');
    body.classList.add('light-mode');
  } else if (theme === 'dark') {
    body.classList.remove('light-mode');
    body.classList.add('dark-mode');
  } else {
    console.warn(`Невідоме значення теми: ${theme}`);
  }
}

