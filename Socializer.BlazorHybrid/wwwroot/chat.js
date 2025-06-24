window.scrollToBottom = (element) => {
    if (element) {
        element.scrollTop = element.scrollHeight;
    }
};

window.isScrollAtBottom = (element) => {
    // Check if scrolled to bottom (within a small threshold)
    return (element.scrollHeight - element.scrollTop - element.clientHeight) < 5;
};
