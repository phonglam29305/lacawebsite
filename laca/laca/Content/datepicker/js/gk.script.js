/*
#------------------------------------------------------------------------
# yourshop.com - November 2010 (for Joomla 1.5)
#
# Copyright (C) 2007-2010 Gavick.com. All Rights Reserved.
# License: Copyrighted Commercial Software
# Website: http://www.gavick.com
# Support: support@gavick.com   
#------------------------------------------------------------------------ 
# Based on T3 Framework
#------------------------------------------------------------------------
# Copyright (C) 2004-2009 J.O.O.M Solutions Co., Ltd. All Rights Reserved.
# @license - GNU/GPL, http://www.gnu.org/copyleft/gpl.html
# Author: J.O.O.M Solutions Co., Ltd
# Websites: http://www.joomlart.com - http://www.joomlancers.com
#------------------------------------------------------------------------
*/

//Call noconflict if detect jquery
//Apply jquery.noConflict if jquery is enabled
if ($defined(window.jQuery) && $type(jQuery.noConflict)=='function') {
	jQuery.noConflict();
	jQuery.noConflict();
}

function switchFontSize (ckname,val){
	var bd = $E('body');
	switch (val) {
		case 'inc':
			if (CurrentFontSize+1 < 7) {
				bd.removeClass('fs'+CurrentFontSize);
				CurrentFontSize++;
				bd.addClass('fs'+CurrentFontSize);
			}		
		break;
		case 'dec':
			if (CurrentFontSize-1 > 0) {
				bd.removeClass('fs'+CurrentFontSize);
				CurrentFontSize--;
				bd.addClass('fs'+CurrentFontSize);
			}		
		break;
		default:
			bd.removeClass('fs'+CurrentFontSize);
			CurrentFontSize = val;
			bd.addClass('fs'+CurrentFontSize);		
	}
	Cookie.set(ckname, CurrentFontSize,{duration:365});
}

function switchTool (ckname, val) {
	createCookie(ckname, val, 365);
	window.location.reload();
}

function createCookie(name,value,days) {
  if (days) {
    var date = new Date();
    date.setTime(date.getTime()+(days*24*60*60*1000));
    var expires = "; expires="+date.toGMTString();
  }else expires = "";
  document.cookie = name+"="+value+expires+"; path=/";
}

//addEvent - attach a function to an event
function gkAddEvent(obj, evType, fn){ 
 if (obj.addEventListener){ 
   obj.addEventListener(evType, fn, false); 
   return true; 
 } else if (obj.attachEvent){ 
   var r = obj.attachEvent("on"+evType, fn); 
   return r; 
 } else { 
   return false; 
 } 
}

window.addEvent('load', function() {
    new SmoothScroll(); 
    var cart_over = false;
    var cart_opened = false;
    // EQUAL COLUMNS
	var equalizers = [$('gk-botsl1'), $('gk-botsl2')];
	equalizers.each(function(wrap,i){
		if(wrap) {
			var max = 0;
			var cols = wrap.getElementsBySelector('.column');
			var col_amount = 0;
			
			cols.each(function(col,j){
				col_amount++;
				if(col.getSize().size.y > max) max = col.getSize().size.y; 
			});
			
			if(col_amount > 1){
				cols.each(function(col){
					col.getElement('div').setStyle("min-height", max + "px");
				});
			}
		}
	});
	//
	if($('gk-cart')) gk_vm_cart_count();
	//
    if($('gk_product_tabs')){
        $('component').addEvent('click', function(e){
            var evt = new Event(e).target;
            
            if((window.ie && evt.nodeName == 'SPAN') || (!window.ie && evt.getTag() == 'span')) {
                if($(evt).getParent().getParent().getProperty('id') == 'gk_product_tabs') {
                    $$('.gk_product_tab').addClass('gk_unvisible');
                    $$('#gk_product_tabs li').setProperty('class', '');
                    var num = 0;
                    $$('#gk_product_tabs li').each(function(el, i){
                        if(el == evt.getParent()){ num = i; }
                    });
                    $$('.gk_product_tab')[num].removeClass('gk_unvisible');
		            $$('#gk_product_tabs li')[num].setProperty('class', 'gk_product_tab_active');
                }
            } else if((window.ie && evt.nodeName == 'LI') || (!window.ie && evt.getTag() == 'li')) {
                if($(evt).getParent().getProperty('id') == 'gk_product_tabs') {
                    $$('.gk_product_tab').addClass('gk_unvisible');
                    $$('#gk_product_tabs li').setProperty('class', '');
                    var num = 0;
                    $$('#gk_product_tabs li').each(function(el, i){
                        if(el == evt.getParent()){ num = i; }
                    });
                    $$('.gk_product_tab')[num].removeClass('gk_unvisible');
		            $$('#gk_product_tabs li')[num].setProperty('class', 'gk_product_tab_active');
                }
            }
        });
    }
    
    if($('gk-items')) {
        var cart_fx = new Fx.Style($('gk-cart'), 'top', { duration:350 });
        /*
        $('gk-items').addEvent('click', function() {
            cart_fx.start((cart_opened) ? -600 : 38);
            cart_opened = !cart_opened;
        });*/
        
        //$('gk-cart').addEvent('mouseenter', function(){ cart_over = true; });
        //$('gk-cart').addEvent('mouseleave', function(){ cart_over = false; });
        /*
        $(document.body).addEvent('click', function(){
            if(cart_opened && !cart_over) cart_fx.start(-600);
        });*/
    }
    
    if($$('.gk_vm_show_cart')[0] && $$('.gk_vm_show_cart')[0].getElement('a')) {
        $$('.gk_vm_show_cart')[0].getElement('a').addClass('button');
    }
    //
    if($('stylearea')){
    	$$('.style_switcher').each(function(element,index){
    		element.addEvent('click',function(event){
                var event = new Event(event);
    			event.preventDefault();
    			changeStyle(index+1);
    		});
    	});
    }
});

// Function to change styles
function changeStyle(style){
	var file = tmplurl+'/css/style'+style+'.css';
	new Asset.css(file);
	new Cookie.set('gk45_style',style,{duration: 200,path: "/"});
	(function(){if(CufonCheck()) Cufon.refresh();}).delay(500);
}

function gk_vm_cart_count(){
	//var amount = 0;
	//$$('#gk-cart .gk_vm_minicart_product div strong').each(function(el){ amount += el.innerHTML.toInt(); });
	//if($$('#gk-items strong')[0]) $$('#gk-items strong')[0].innerHTML = amount;
}

// VM function override
function handleAddToCart( formId, parameters ) {
	formCartAdd = document.getElementById( formId );
	
	var callback = function(responseText) {
		updateMiniCarts();
		// close an existing mooPrompt box first, before attempting to create a new one (thanks wellsie!)
		if (document.boxB) {
			document.boxB.close();
			clearTimeout(timeoutID);
		}

		document.boxB = new MooPrompt(notice_lbl, responseText, {
				buttons: 2,
				width:400,
				height:150,
				overlay: false,
				button1: ok_lbl,
				button2: cart_title,
				onButton2: 	handleGoToCart
			});
			
		setTimeout( 'document.boxB.close()', 3000 );
	}
	
	var opt = {
	    // Use POST
	    method: 'post',
	    // Send this lovely data
	    data: $(formId),
	    // Handle successful response
	    onComplete: callback,
	    
	    evalScripts: true
	}

	new Ajax(formCartAdd.action, opt).request();
	
	(function(){if($('gk-cart')) gk_vm_cart_count();}).delay(3000);
}

function CufonCheck(){ return (typeof(Cufon) == "undefined")?  false: true; }

// JCaptionCheck
function JCaptionCheck(){ return (typeof(JCaption) == "undefined")?  false: true; }

if(!JCaptionCheck()) {
	var JCaption = new Class({
		initialize: function(selector)
		{
			this.selector = selector;
			var images = $$(selector);
			images.each(function(image){ this.createCaption(image); }, this);
		},

		createCaption: function(element)
		{
			var caption   = document.createTextNode(element.title);
			var container = document.createElement("div");
			var text      = document.createElement("p");
			var width     = element.getAttribute("width");
			var align     = element.getAttribute("align");
			var docMode = document.documentMode;

			//Windows fix
			if (!align)
				align = element.getStyle("float");  // Rest of the world fix
			if (!align) // IE DOM Fix
				align = element.style.styleFloat;

			text.appendChild(caption);
			text.className = this.selector.replace('.', '_');

			if (align=="none") {
				if (element.title != "") {
					element.parentNode.replaceChild(text, element);
					text.parentNode.insertBefore(element, text);
				}
			} else {
				element.parentNode.insertBefore(container, element);
				container.appendChild(element);
				if ( element.title != "" ) {
					container.appendChild(text);
				}
				container.className   = this.selector.replace('.', '_');
				container.className   = container.className + " " + align;
				container.setAttribute("style","float:"+align);

				//IE8 fix
				if (!docMode|| docMode < 8) {
					container.style.width = width + "px";
				}
			}

		}
	});

	document.caption = null;
	window.addEvent('load', function() {
		var caption = new JCaption('img.caption')
		document.caption = caption
	});

}
