window.addEvent('load', function() {
    if(MooTools.version.contains('1.1')){
        $$(".gk_is_wrapper-template").each(function(el){
            var elID = el.getProperty('id');
            new GK_IS_nov2010_11($Gavick[elID], el, elID);
        });
    } else {
        $$(".gk_is_wrapper-template").each(function(el){
            var elID = el.getProperty('id');
            new GK_IS_nov2010_12($Gavick[elID], el, elID);
        });
	}
});

var GK_IS_nov2010_11 = new Class({
    
    options: {
        "anim_type" : "opacity", 
        "anim_interval" : 5000,
        "anim_speed" : 500,
        "slide_links" : true
    },
    
    initialize: function(options, el, elID) {
        this.setOptions(options);
        $this = this;
        this.loadedImages = false;
        this.wrapper = $(elID);   
        this.slides = [];
        this.contents = [];
        this.links = [];
        this.play = false;
        this.overlayAnim = false;
        this.actual_slide = 0;
        this.wrap_width = $E('.gk_is_slides', this.wrapper).getSize().size.x;
        this.wrap_height = $E('.gk_is_slides', this.wrapper).getSize().size.y;
        this.text = $E('.gk_is_text', this.wrapper);
        this.blank = false;
        this.playing = false;
        
        $ES('.gk_is_text_item', this.wrapper).each(function(el){
            $this.links.push(el.getElement('h4 a').getProperty('href'));
        });
        
        if(this.text) this.text_anim(0);
        var imagesToLoad = [];
        var amount = $ES('.gk_is_slide', this.wrapper).length;
        
        $ES('.gk_is_slide', this.wrapper).each(function(elm,i){
            $this.links.push(elm.innerHTML);
            var newImg = new Element('img', { 'class' : 'gk_is_slide', 'alt' : '', 'src' : elm.innerHTML, 'style' : 'z-index:' + (amount - i) });
            imagesToLoad.push(newImg);
            newImg.injectAfter(elm);
            elm.remove();
        });
                
        var time = (function(){
            var process = 0;                
            imagesToLoad.each(function(el,i){ if(el.complete) process++; });
            
            if(process == imagesToLoad.length){
                $clear(time);
                $this.loadedImages = process;
                new Fx.Style($E('.gk_is_preloader', $this.wrapper), 'opacity').start(0); 
                $ES('.gk_is_slide', $this.wrapper).each(function(slide,i){
                	$this.slides.push({
                		"slide":slide, 
                		"anim": new Fx.Style(slide, 'opacity', { duration: $this.options['anim_speed'] }).set(i != 0 ? 0 : 1)
                	});
                	if($this.options.slide_links) slide.addEvent('click', function() { $this.redirect($this.actual_slide); });
                });
                
                $E('.gk_is_prev_btn', $this.wrapper).addEvent('click', function() {
                    if(!$this.playing) {
                        $this.anim($this.actual_slide-1);
                        $this.blank = true;
                    }
                });
                
                $E('.gk_is_next_btn', $this.wrapper).addEvent('click', function() {
                    if(!$this.playing) {
                        $this.anim($this.actual_slide+1);
                        $this.blank = true;
                    }
                });
            }
        }).periodical(200);
        
        if(this.options['autoanim']){
            (function(){
                if($this.blank == false){
                    $this.anim($this.actual_slide+1);
                }else $this.blank = false;
            }).periodical($this.options['anim_interval']+$this.options['anim_speed']);
        }
    },
    
    text_anim: function(which) {
        var max = $this.slides.length-1;
        which = (which > max) ? 0 : ((which < 0) ? max : which);
        
        $E('.gk_is_text', $this.wrapper).innerHTML = $ES('.gk_is_text_item', $this.wrapper)[which].innerHTML;
    },
    
    anim: function(which) {
        if(which != $this.actual_slide){ 
            var max = $this.slides.length-1;
            which = (which > max) ? 0 : ((which < 0) ? max : which);
            var actual = $this.actual_slide;
            $this.actual_slide = which;
            
            $this.playing = true;
            
            if($this.text) $this.text_anim(which);
            // opacity anim
            $this.slides[actual].slide.setStyle("z-index",max+1);
            $this.slides[actual].anim.start(1,0);
            $this.slides[which].anim.start(0,1);
            
            (function(){
                $this.slides[$this.actual_slide].slide.setStyle("z-index", $this.actual_slide);
                $this.playing = false;
            }).delay($this.options['anim_speed']);
        }
    },
    
    redirect: function(where) {
        window.location = $this.links[where];
    }
});

GK_IS_nov2010_11.implement(new Options);

var GK_IS_nov2010_12 = new Class({
    
    options: {
        "anim_type" : "opacity", 
        "anim_interval" : 5000,
        "anim_speed" : 500,
        "slide_links" : true
    },
    
    initialize: function(options, el, elID) {
        this.setOptions(options);
        $this = this;
        this.loadedImages = false;
        this.wrapper = $(elID);   
        this.slides = [];
        this.contents = [];
        this.links = [];
        this.play = false;
        this.overlayAnim = false;
        this.actual_slide = 0;
        this.stripes = [];
        this.wrap_width = this.wrapper.getElement('.gk_is_slides').getSize().x;
        this.wrap_height = this.wrapper.getElement('.gk_is_slides').getSize().y;
        this.text = this.wrapper.getElement('.gk_is_text');
        this.blank = false;
        this.playing = false;
        
        this.wrapper.getElements('.gk_is_text_item').each(function(el){
            $this.links.push(el.getElement('h4 a').getProperty('href'));
        });
        
        if(this.text) this.text_anim(0);
        var imagesToLoad = [];
        var amount = this.wrapper.getElements('.gk_is_slide').length;
        
        this.wrapper.getElements('.gk_is_slide').each(function(elm,i){
            $this.links.push(elm.innerHTML);
            var newImg = new Element('img', { 'class' : 'gk_is_slide', 'alt' : '', 'src' : elm.innerHTML, 'style' : 'z-index:' + (amount - i) });
            imagesToLoad.push(newImg);
            newImg.injectAfter(elm);
            elm.remove();
        });
        
        if($this.options.anim_type != 'opacity') this.init_stripes();
              
        var time = (function(){
            var process = 0;                
            imagesToLoad.each(function(el,i){ if(el.complete) process++; });
            
            if(process == imagesToLoad.length){
                $clear(time);
                $this.loadedImages = process;
                (function(){
                    new Fx.Style($this.wrapper.getElement('.gk_is_preloader'), 'opacity').start(0); 
                    $this.wrapper.getElements('.gk_is_slide').each(function(slide,i){
                    	$this.slides.push({
                    		"slide":slide, 
                    		"anim": new Fx.Style(slide, 'opacity', { duration: $this.options['anim_speed'] }).set(i != 0 ? 0 : 1)
                    	});
                    	if($this.options.slide_links) slide.addEvent('click', function() { $this.redirect($this.actual_slide); });
                    });
                }).delay(400);
                
                                
                $this.wrapper.getElement('.gk_is_prev_btn').addEvent('click', function() {
                    if(!$this.playing) {
                        $this.anim($this.actual_slide-1);
                        $this.blank = true;
                    }
                });
                
                $this.wrapper.getElement('.gk_is_next_btn').addEvent('click', function() {
                    if(!$this.playing) {
                        $this.anim($this.actual_slide+1);
                        $this.blank = true;
                    }
                });
            }
        }).periodical(200);
        
        if(this.options['autoanim']){
            (function(){
                if($this.blank == false){
                    $this.anim($this.actual_slide+1);
                }else $this.blank = false;
            }).periodical($this.options['anim_interval']+$this.options['anim_speed']);
        }
    },
    
    text_anim: function(which) {
        var max = $this.slides.length-1;
        which = (which > max) ? 0 : ((which < 0) ? max : which);
        
        $this.wrapper.getElement('.gk_is_text').innerHTML = $this.wrapper.getElements('.gk_is_text_item')[which].innerHTML;
    },
    
    anim: function(which) {
        if(which != $this.actual_slide){ 
            var max = $this.slides.length-1;
            which = (which > max) ? 0 : ((which < 0) ? max : which);
            var actual = $this.actual_slide;
            $this.actual_slide = which;
            
            $this.playing = true;
            
            if($this.text) $this.text_anim(which);
            // opacity anim
           	if($this.options.anim_type == 'opacity') {
	            $this.slides[actual].slide.setStyle("z-index",max+1);
	            $this.slides[actual].anim.start(1,0);
	            $this.slides[which].anim.start(0,1);
	            
	            (function(){
	                $this.slides[$this.actual_slide].slide.setStyle("z-index", $this.actual_slide);
	                $this.playing = false;
	            }).delay($this.options['anim_speed']);
	        }
        }
    },
    
    redirect: function(where) {
        window.location = $this.links[where];
    }
});

GK_IS_nov2010_12.implement(new Options);