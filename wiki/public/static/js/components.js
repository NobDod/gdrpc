class TButton extends HTMLElement {
    constructor() {
        super()
        const bgtype = !this.getAttribute("type") ? "solid" : this.getAttribute("type"), 
            col = !this.getAttribute("color") ? "green"   : this.getAttribute("color");
        if(bgtype == "solid") {this.classList.add("bg-" + col + "-500", "hover:bg-" + col + "-400", "text-white")}
        else if(bgtype == "outline") {this.classList.add("bg-transparent", "hover:bg-" + col + "-800", "text-" + col + "-200", "border", "border-" + col + "-500")}
        else if(bgtype == "light") { this.classList.add("bg-" + col + "-100", "hover:bg-" + col + "-200", "text-" + col + "-500")  }
        this.classList.add("font-medium", "px-4", "py-2", "rounded", "transition", "duration-300", "cursor-pointer", `relative`)
    }
}

class TAlerts extends HTMLElement {
    constructor() {
        super()
        var col = !this.getAttribute("type") ? "error" : this.getAttribute("type");
        if(col != "error" && col != "warning" && col != "success" && col != "info"){
            col = "info";
        }
        var color;
        if(col == "error"){color = "red";}
        if(col == "success"){color = "green";}
        if(col == "info"){color = "blue";}
        if(col == "warning"){color = "yellow";}
        this.id = !this.id ? "alert"+Math.floor(Math.random() * Math.floor(5000000)) : this.id;
        this.classList.add(`block`, `text-sm`, `text-${color}-600`, `bg-${color}-200`, `border`, `border-${color}-400`, 
        `h-12`, `flex`, `items-center`, `p-4`, `rounded-sm`, `relative`);
        this.innerHTML = `<strong class="mr-1">${this.innerHTML}</strong>`;
        if(!this.getAttribute("no_close")){
            this.innerHTML += `<button type="button" data-dismiss="alert" aria-label="Close" onclick="document.getElementById('${this.id}').remove();">
    <span class="absolute top-0 bottom-0 right-0 text-2xl px-3 py-1 hover:text-${color}-900" aria-hidden="true" >×</span>
</button>`;
        }
    }
}

/**
 * not to custom elemtns
 */
class ModalSystem{
    /*
     * @param element - document
    */
    static createModal(title = ``, text = ``, buttons = ``, noClose = false){
        var elem = document.createElement("div");
        elem.id = 'modal-'+Random.string(6)+"-"+Random.string(32);
        elem.classList.add("hidden");
        elem.innerHTML = `<div class="flex items-center justify-center fixed left-0 bottom-0 w-full h-full bg-gray-800 bg-opacity-80">
<div class="bg-gray-900 rounded-lg w-auto">
    <div class="flex flex-col items-start p-4">
        <div class="flex items-center w-full">
            <div class="text-white font-medium text-lg">${title}</div>
            <svg class="ml-auto fill-current text-white w-6 h-6 cursor-pointer" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 18 18" id="${elem.id}_cbtn">
                <path d="M14.53 4.53l-1.06-1.06L9 7.94 4.53 3.47 3.47 4.53 7.94 9l-4.47 4.47 1.06 1.06L9 10.06l4.47 4.47 1.06-1.06L10.06 9z"/>
            </svg>
        </div>
        <hr><div class="text-white">${text}</div><hr>
        <div class="ml-auto">${buttons}</div>
    </div>
</div></div>`;
document.getElementById("body").appendChild(elem);
        if(!noClose){
            document.getElementById(`${elem.id}_cbtn`).onclick = function(){
                ModalSystem.removeWithAnim(elem.id);
            }
            document.getElementById(`${elem.id}_cbtn`).id = "";
        }else{
            document.getElementById(`${elem.id}_cbtn`).remove();
        }
        return elem.id;
    }

    static showWithAnim(modalId){
        $(`#${modalId}`).removeClass('hidden');
        $(`#${modalId}`).css('display','none');
        $(`#${modalId}`).fadeIn(200);
    }

    static removeWithAnim(modalId){
        $(`#${modalId}`).fadeOut(200, function(){$(`#${modalId}`).remove()});
    }
}
class ModalHelper{
    static createLoader(noClose = false){
        var id = ModalSystem.createModal(``, `<svg version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"
        width="60px" height="60px" viewBox="0 0 40 40" enable-background="new 0 0 40 40" xml:space="preserve">
        <path opacity="0.2" fill="#fff" d="M20.201,5.169c-8.254,0-14.946,6.692-14.946,14.946c0,8.255,6.692,14.946,14.946,14.946
            s14.946-6.691,14.946-14.946C35.146,11.861,28.455,5.169,20.201,5.169z M20.201,31.749c-6.425,0-11.634-5.208-11.634-11.634
            c0-6.425,5.209-11.634,11.634-11.634c6.425,0,11.633,5.209,11.633,11.634C31.834,26.541,26.626,31.749,20.201,31.749z"/>
            <path fill="#fff" d="M26.013,10.047l1.654-2.866c-2.198-1.272-4.743-2.012-7.466-2.012h0v3.312h0
            C22.32,8.481,24.301,9.057,26.013,10.047z">
            <animateTransform attributeType="xml"
                attributeName="transform"
                type="rotate"
                from="0 20 20"
                to="360 20 20"
                dur="0.5s"
                repeatCount="indefinite"/>
        </path>
    </svg>`,``,noClose);
        ModalSystem.showWithAnim(id);
        return id;
    }
}

customElements.define('t-button', TButton);
customElements.define('t-alert', TAlerts);