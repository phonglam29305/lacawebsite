window.addEvent('domready', function() {
			
			var container = 'miniK2StoreCart';
			var cartcontainer = '.cart_form';
			
			if ($(container))	{
				doMiniCart();
			}
			
			//only initialise when there is a cart form present
			if ($$(cartcontainer))	{
			      
		    // SqueezeBox.initialize({});
		      $$('.cart_form').each(function(el) {
		         el.addEvent('submit', function(e) {
		            new Event(e).stop();
		            var quantity = this.quantity.value;
		            var url = this.action+'&quantity='+quantity+'&Itemid='+this.Itemid.value;
		            new Ajax(url,{
						 method:"post",
						 onSuccess: function(response){
							SqueezeBox.applyContent(response);
							if ($(container))	{
							doMiniCart();
							}
						 }
					
					}).request();
					
		         });
		         
				});
			}
		      
		 }); //end dom ready
		 
		function doMiniCart() {
		var container = 'miniK2StoreCart';
			var murl = k2storeURL+'index.php?option=com_k2store&view=mycart&format=ajaxmini';
		
			var a=new Ajax(murl,{
                method:"post",
                onComplete: function(response){
                    $(container).setHTML(response); 
					}
				}).request();
			}


function k2storeGetPaymentForm( element, container )
{
    var url = k2storeURL+'index.php?option=com_k2store&view=billing&task=getPaymentForm&format=raw&payment_element=' + element;
    k2storeDoTask( url, container, document.adminForm );
}


function k2storeDoTask( url, container, form, msg ) 
	{
		k2storeNewModal(msg);
		
    	// if url is present, do validation
		if (url && form) 
		{	
			// loop through form elements and prepare an array of objects for passing to server
			var str = new Array();
			for(i=0; i<form.elements.length; i++)
			{
				postvar = {
					name : form.elements[i].name,
					value : form.elements[i].value,
					checked : form.elements[i].checked,
					id : form.elements[i].id
				};
				str[i] = postvar;
			}
			// execute Ajax request to server
            var a=new Ajax(url,{
                method:"post",
				data:{"elements":Json.toString(str)},
                onComplete: function(response){
                    var resp=Json.evaluate(response, false);
                    if ($(container)) { $(container).setHTML(resp.msg); }
                    (function() { document.body.removeChild($('k2storeModal')); }).delay(500);
                    return true;
                }
            }).request();
		}
			else if (url && !form) 
		{
			// execute Ajax request to server
            var a=new Ajax(url,{
                method:"post",
                onComplete: function(response){
                    var resp=Json.evaluate(response, false);
                    if ($(container)) { $(container).setHTML(resp.msg); }
                    (function() { document.body.removeChild($('k2storeModal')); }).delay(500);
            }
            }).request();			
		}
	}

	/**
	 * 
	 * @param {String} msg message for the modal div (optional)
	 */
	function k2storeNewModal (msg)
	{
	    if (typeof window.innerWidth != 'undefined') {
	        var h = window.innerHeight;
	        var w = window.innerWidth;
	    } else {
	        var h = document.documentElement.clientHeight;
	        var w = document.documentElement.clientWidth;
	    }
	    var t = (h / 2) - 15;
	    var l = (w / 2) - 15;
		var i = document.createElement('img');
		var s = window.location.toString();
		var src = 'components/com_k2store/images/ajax-loader.gif';
		i.src = (s.match(/administrator\/index.php/)) ? '../' + src : src;
		i.style.position = 'absolute';
		i.style.top = t + 'px';
		i.style.left = l + 'px';
		i.style.backgroundColor = '#000000';
		i.style.zIndex = '100001';
		var d = document.createElement('div');
		d.id = 'k2storeModal';
		d.style.position = 'fixed';
		d.style.top = '0px';
		d.style.left = '0px';
		d.style.width = w + 'px';
		d.style.height = h + 'px';
		d.style.backgroundColor = '#000000';
		d.style.opacity = 0.5;
		d.style.filter = 'alpha(opacity=50)';
		d.style.zIndex = '100000';
		d.appendChild(i);
	    if (msg != '' && msg != null) {
		    var m = document.createElement('div');
		    m.style.position = 'absolute';
		    m.style.width = '200px';
		    m.style.top = t + 50 + 'px';
		    m.style.left = (w / 2) - 100 + 'px';
		    m.style.textAlign = 'center';
		    m.style.zIndex = '100002';
		    m.style.fontSize = '1.2em';
		    m.style.color = '#ffffff';
		    m.innerHTML = msg;
		    d.appendChild(m);
		}
		document.body.appendChild(d);
	}
