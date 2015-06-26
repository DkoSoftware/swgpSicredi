MessageBox = {
    queue: new Array(),
    showNext: function(){
        if (MessageBox.queue.length > 0) {
            var fn = MessageBox.queue.shift();
            fn();
        }
    }
}