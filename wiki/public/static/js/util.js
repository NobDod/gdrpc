function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

const Cookie = {
    getCookie: function(name) {
        let matches = document.cookie.match(new RegExp(
          "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
        ));
        return matches ? decodeURIComponent(matches[1]) : undefined;
    },
    setCookie: function(name, value, options = {}) {
        options = {
            path: '/',
            ...options
        };
        if (options.expires instanceof Date) {
            options.expires = options.expires.toUTCString();
        }
        let updatedCookie = encodeURIComponent(name) + "=" + encodeURIComponent(value);
        for (let optionKey in options) {
            updatedCookie += "; " + optionKey;
            let optionValue = options[optionKey];
            if (optionValue !== true) {
                updatedCookie += "=" + optionValue;
            }
        }
        document.cookie = updatedCookie;
        return true;
    },
    deleteCookie: function(name) {
        return setCookie(name, "", {
          'max-age': -1
        });
    }
}

function dateParser(date) {
    var seconds = Math.floor((new Date() - date) / 1000);
    var interval = seconds / 31536000;
    var ago = " назад";
    if (interval > 1) {
        if(Math.floor(interval) == 1) {
            return Math.floor(interval) + " год" + ago;
        }
        else if(Math.floor(interval) < 5) {
            return Math.floor(interval) + " года" + ago;
        }
        else {
            return Math.floor(interval) + " лет" + ago;
        }
    }
    interval = seconds / 2592000;
    if (interval > 1) {
        if(Math.floor(interval) == 1) {
            return Math.floor(interval) + " месяц" + ago;
        }
        else if(Math.floor(interval) < 5) {
            return Math.floor(interval) + " месяц" + ago;
        }
        else {
            return Math.floor(interval) + " месяцев" + ago;
        }
    }
    interval = seconds / 86400;
    if (interval > 1) {
        if(Math.floor(interval) == 1) {
            return Math.floor(interval) + " день" + ago;
        }
        else if(Math.floor(interval) < 5) {
            return Math.floor(interval) + " дня" + ago;
        }
        else {
            return Math.floor(interval) + " дней" + ago;
        }
    }
    interval = seconds / 3600;
    if (interval > 1) {
        if(Math.floor(interval) == 1) {
            return Math.floor(interval) + " час" + ago;
        }
        else if(Math.floor(interval) < 5) {
            return Math.floor(interval) + " часа" + ago;
        }
        else {
            return Math.floor(interval) + " часов" + ago;
        }
    }
    interval = seconds / 60;
    if (interval > 1) {
        if(Math.floor(interval) == 1) {
            return Math.floor(interval) + " минуту" + ago;
        }
        else if(Math.floor(interval) < 5) {
            return Math.floor(interval) + " минуты" + ago;
        }
        else {
            return Math.floor(interval) + " минут" + ago;
        }
    }
    if(Math.floor(interval) == 1) {
        return Math.floor(interval) + " секунду" + ago;
    }
    else if(Math.floor(interval) < 5) {
        return Math.floor(interval) + " секунды" + ago;
    }
    else {
        return Math.floor(interval) + " секунд" + ago;
    }
}

const Random = {
    int: function(min, max) {
        min = Math.ceil(min);
        max = Math.floor(max);
        return Math.floor(Math.random() * (max - min)) + min;
    },
    string: function(length) {
        var result = '', characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        for ( var i = 0; i < length; i++ ) {
            result += characters.charAt(Math.floor(Math.random() * characters.length));
        }
        return result;
    }
}

const Temp = {
    _temp: [],
    
    clear: function(){
        Temp._temp = [];
    },
    
    del: function(universal_content){
        const content = universal_content.split(':');
        try { 
            Temp._temp[content[0]][content[1]] = undefined; 
            if(Temp._temp[content[0]].length < 1){
                Temp._temp[content[0]] = undefined;
            } 
            return true;
        } catch (error) { return false; }
    },

    get: function(universal_content){
        const content = universal_content.split(':');
        try {
            if(typeof Temp._temp[content[0]][content[1]] !== 'undefined'){
                return Temp._temp[content[0]][content[1]];
            }
            throw 'no';
        } catch (error) {
            return null;
        }
    },

    edit: function(universal_content, data){
        const content = universal_content.split(':');
        try { 
            if(typeof Temp._temp[content[0]][content[1]] !== 'undefined'){
                Temp._temp[content[0]][content[1]] = data;
            }
            return true;
        } catch (error) { return false; }
    },

    add: function(data){
        const id2 = Random.string(Random.int(16,32)), id1 = Random.int(1,5);
        try {
            if(typeof Temp._temp[id1] !== 'object'){Temp._temp[id1]=[];}
            Temp._temp[id1][id2] = data;
            return `${id1}:${id2}`;   
        } catch (error) {
            return false;
        }
    }
}

class Google{
    static reCaptcha(callback){
        var btn_id = "al-"+Random.string(16);
        var id = ModalSystem.createModal("Я не робот", 
`<div id="${btn_id}"></div>`, ``);
        grecaptcha.render(btn_id, {
            'sitekey' : '6LctOgEVAAAAACDF1vW3qeRCp-XLumMGJ9oV_mYF',
            'callback' : function(){
                const token = grecaptcha.getResponse();
                if(token.length < 16){
                    grecaptcha.reset();
                    return;
                }
                ModalSystem.removeWithAnim(id);
                callback(token);
            },
            'theme' : 'dark'
        });
        ModalSystem.showWithAnim(id);
        return id;
    }
}