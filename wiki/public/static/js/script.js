$(document).ready(async function(){
    //end
    await Router.Init();
    $(document).on('click', 'a', function(e){
        const h = e.target.href;
        const redirect = e.target.pathname;
        if(h.startsWith("mailto://")){
            return true;
        }
        log("[Window] location changed: " + redirect);
        e.preventDefault();
        e.stopImmediatePropagation();
        Router.changeUrl(redirect);
        Router.Run(redirect);
        return false;
    });
   
    log('site ready');
    Temp.clear();
    Router.Run('');
    Loader.HideLoader();
    Loader.ShowContent();
});