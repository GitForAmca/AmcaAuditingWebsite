//Menu
$(document).ready(function(){
  //the trigger on hover when cursor directed to this class
    $(".core-menu li").hover(
    function(){
      //i used the parent ul to show submenu
        $(this).children('ul').slideDown(50);
    }, 
      //when the cursor away 
    function () {
        $('ul', this).slideUp(50);
    });
});

///

/*Service Slider*/
$(document).ready(function(){
    $(".serviceitem").owlCarousel({
     items : 4,
     autoplay: true,
     loop  : false,
     margin : 10,
     dots: false,
     nav    : false,
     navText: ["<i class='las la-long-arrow-alt-left'>", "<i class='las la-long-arrow-alt-right'>"],
     responsiveClass:true,
     responsive:{
        0:{
            items:1,
        },
        600:{
            items:2,
         },
         1024: {
             items: 3,
         },
         1200: {
             items: 4,
         }
    }
  });
});
/*Sub Service Slider*/
$(document).ready(function(){
  $(".sub-services").owlCarousel({
     items : 2,
     autoplay: true,
     loop  : false,
     margin : 10,
     dots: false,
     nav    : true,
     navText: ["<i class='las la-long-arrow-alt-left'>", "<i class='las la-long-arrow-alt-right'>"],
     responsiveClass:true,
     responsive:{
        0:{
            items:1,
        },
        600:{
            items:2,
        },
    }
  });
});
/*Testimonials Slider*/
$(document).ready(function(){
  $(".testimonials").owlCarousel({
     items : 1,
     autoplay: true,
     loop  : true,
     margin : 20,
     dots: false,
     nav    : true,
     navText: ["<i class='las la-long-arrow-alt-left'>", "<i class='las la-long-arrow-alt-right'>"],
     responsiveClass:true,
     responsive:{
        0:{
            items:1,
        },
        600:{
            items:1,
        },
    }
  });
});
/*Blogs Slider*/
$(document).ready(function(){
  $(".blogsfeedsSlider").owlCarousel({
     items : 1,
     autoplay: true,
     loop  : true,
     margin : 0,
     dots: false,
     nav    : true,
     navText: ["<i class='las la-long-arrow-alt-left'>", "<i class='las la-long-arrow-alt-right'>"],
     responsiveClass:true,
     responsive:{
        0:{
            items:1,
        },
        600:{
            items:1,
        },
    }
  });
});
/*Client Slider*/
$(document).ready(function(){
    $(".clients-carousel").owlCarousel({
     items : 7,
     autoplay: true,
     loop  : true,
     margin : 30,
     dots: false,
        nav: false,
     navText: ["<i class='las la-long-arrow-alt-left'>", "<i class='las la-long-arrow-alt-right'>"],
     responsiveClass:true,
     responsive:{
        0:{
            items:2,
        },
        600:{
            items:7,
        },
    }
  });
});

//////////////////

$('#menu-open').click(function(){
    $('.menu-wrapper').css('display','block');
    $('body').css('overflow','hidden');
});
$('#menu-close').click(function(){
    $('.menu-wrapper').css('display','none');
    $('body').css('overflow-y','scroll');
});

/////////////////

$('.hoverClass').mouseenter(function(){
    $(this).children('a').css('color','#000000');
})
$('.hoverClass').mouseleave(function(){
    $(this).children('a').css('color','#9d9ca4');
})
$('.hoverClassdrop').mouseenter(function(){
    $(this).children('a').css('background','#bdbdbd');
})
$('.hoverClassdrop').mouseleave(function(){
    $(this).children('a').css('background','transparent');
})
/*Scrolling Effects*/
$('#requestCust').on('click', function () {
    $('html, body').animate({
        scrollTop: $('#requestWrapper').offset().top
    }, 1000)
});

//URL fetching
$(".eachJob").mousedown(function (o) {
    switch (o.which) {
        case 1: var t = $(this).attr("id");
            $.ajax({
                type: "post",
                url: "/Career/getdata",
                data: { JobID: this.id },
                success: function (o) {
                    history.pushState("", document.title, window.location.origin + "/" + o);
                    location.reload();
                }
            })
            break;
        case 2: t = $(this).attr("id");
            $.ajax({
                type: "post",
                url: "/Career/getdata",
                data: { JobID: this.id },
                success: function (o) {
                    window.open(window.location.origin + "/" + o, "_blank")

                }
            })
    }
});

$(".eachBlog").mousedown(function (o) {
    switch (o.which) {
        case 1: var t = $(this).attr("id");
            $.ajax({
                type: "post",
                url: "/Insight/getdata",
                data: { blogID: t },
                success: function (o) {
                    history.pushState("", document.title, window.location.origin + "/" + o);
                    location.reload();
                }
            });
            break;
        case 2: t = $(this).attr("id");
            $.ajax({
                type: "post",
                url: "/Insight/getdata",
                data: { blogID: t },
                success: function (o) {
                    window.open(window.location.origin + "/" + o, "_blank")
                }
            })
    }
});

$('.restrictZero').keyup(function () {
    var value = $(this).val();
    value = value.replace(/^(0*)/, "");
    value = value.replace(/[^0-1-2-3-4-5-6-7-8-9\s]/g, '');
    $(this).val(value);
});
var captchaError = $('#captcha p').text().length;
if (captchaError < 2) {
    $(document).ready(function () {
        var b = "+971";
        $('#CountryCodeContact option:selected').val(b);
        $('#CountryCodeContact option')[0].value = b;
        $('#CountryCodeContact option')[0].innerHTML = $('#CountryCodeContact option:selected').val();
        $("#CountryCodeContact").val($('#CountryCodeContact option:selected').val());
        $('#CountryCodeContact').change(function () {
            var a = $('#CountryCodeContact option:selected').val();
            $('#CountryCodeContact option')[0].value = a;
            $('#CountryCodeContact option')[0].innerHTML = $('#CountryCodeContact option:selected').val();
            $("#CountryCodeContact").val($('#CountryCodeContact option:selected').val());
        });
    });
    
}


$('#CaptchaInputText').addClass('form-control forminput-theme');
$("img#CaptchaImage").attr({ "alt": "CaptchaImage", "width": "100", "height": "35"});
/*search in Dropdown*/
var searchSelectList = $('.search-SelectList');
if (searchSelectList.length > 0) {
       
        $('.search-SelectList').change(function () {
                    
                var a = $('.search-SelectList option:selected').val();
                setTimeout(function () {
                    document.getElementsByClassName('search-SelectList')[0].getElementsByTagName('button')[0].getElementsByTagName('span')[0].innerText = a;

                }, 100);

         });

     
}

/*to make input autocomplete off*/
$(document).ready(function () {
    var TypeText = $('input');
    TypeText.attr('autocomplete', 'off');
});
    
