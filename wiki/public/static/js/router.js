/**
 * Change logs
 */
function XtJTGK4vGubt4pUT5Zw6QUNreWdGsHDp(a, b) {
    if(a == console.log || a == console.error || a == console.warn || a == console.info){
        if(b.length > 1){ a(b);}else{ a(b[0].toString()); }
    }
}
function log(...param){XtJTGK4vGubt4pUT5Zw6QUNreWdGsHDp(console.log, param);}
function warn(...param){XtJTGK4vGubt4pUT5Zw6QUNreWdGsHDp(console.warn, param);}
function error(...param){XtJTGK4vGubt4pUT5Zw6QUNreWdGsHDp(console.error, param);}
function info(...param){XtJTGK4vGubt4pUT5Zw6QUNreWdGsHDp(console.info, param);}

log("Hopix Loader inited");

class Loader{
    /* Helper */
    static HideLoader(){
        Loader.#ScripterHide("#loader");
        Loader.#ScripterShow("#body");
    }
    static ShowLoader(){
        Loader.#ScripterShow("#loader");
        Loader.#ScripterHide("#body");
    }
    static ShowContent(){
        Loader.#ScripterHide("#content_loader",function(){
            Loader.#ScripterShow("#content");
        }, 50);
    }
    static HideContent(){
        Loader.#ScripterHide("#content",function(){
            Loader.#ScripterShow("#content_loader");
        }, 50);
    }
    
    static Text(text=""){
        $("#load_text").html(text);
    }

    /* Main Function */
    static #ScripterShow(a,c=null,s=200){
        $(a).removeClass('hidden');
        $(a).css('display','none');
        $(a).fadeIn(s,function(){
            if(c){c();}
        });
    }
    static #ScripterHide(a,c=null,s=200){
        $(a).fadeOut(s,function(){
            $(a).addClass('hidden');
            $(a).attr("style","");
            if(c){c();}
        });
    }
}
class Router{
    /* Utils */
    static getUrl(url=""){
        if(!url) url = window.location.pathname;
        while(url.endsWith('/')){
            url = url.slice(0,-1);
        }
        while(url.startsWith('/')){
            url = url.slice(1);
        }
        return url;
    }
    static changeUrl(toNew){
        var url = Router.getUrl(toNew);
        window.history.pushState({urlPath:`/${url}`},"",`/${url}`);
    }

    /* Router */
    static #routes = [];
    static #args = [];
    static #hs_c = undefined;

    static changePage(url){
        Router.changeUrl(url);
        Router.Run(url);
    }

    static async Init(){
        const content = await $.ajax('/static/pages.txt?_=' + new Date().getTime());
        const routes = content.split('\n');
        for(var i = 0; i < routes.length; i++){
            const route = routes[i].split('|');
            for(var j = 0; j < route.length; j++){
                while(route[j].endsWith(' ')){route[j] = route[j].slice(0,-1); } 
                while(route[j].startsWith(' ')){route[j] = route[j].slice(1);}
            }
            Router.#routes.push({
                url: new RegExp('^' + Router.getUrl(route[0]).replace(/:\w+/g,'(\\w+)') + '$'),
                run: route[1]
            });
        }
        log("routes: " + routes.length);

        
        window.onhashchange = function(){
            if(!Router.HashRun()){
                warn("rhash not runned");
                return;
            }
        };
        log("rhash registered");
    }

    static async Run(path=""){
        var e = document.getElementById("content");
        Router.#args = [];
        path = Router.getUrl(path);
        var c = false, arg = "";

        //find this route
        for(var i = 0; i < Router.#routes.length; i++){
            const data = Router.#routes[i];
            var args = path.match(data.url);
            if(args) {
                Router.#args = args;
                c = data;
                break;
            }
        }
        if(!c){
            e.innerHTML = "Не найдено.";
        }else{
            var a = "";
            try{
                a = await $.ajax("/static/pages/" + c.run + "?_=" + new Date().getTime());
            }catch(b){
                a = b.responseText;
            }
            e.innerHTML = a;
        }
        const scripts = e.getElementsByTagName("script");
        for(var i = 0; i < scripts.length; i++){
            if(!scripts[i].src){
                try {
                    new Function(scripts[i].innerText)();
                } catch (error) {
                    log('route ('+path+') eval error: ' + error);
                }
                scripts[i].remove();
                continue;
            }
        }
    }
}