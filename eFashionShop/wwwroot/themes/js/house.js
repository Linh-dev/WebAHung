/* ==Contact Form================== */


$(window).scroll(function(){
    if($(window).scrollTop() > 300){
        $("#back-to-top").fadeIn(600);
    } else{
        $("#back-to-top").fadeOut(600);
    }
});

$('#back-to-top, .back-to-top').click(function() {
      $('html, body').animate({ scrollTop:0 }, '1000');
      return false;
});

new WOW().init();
$(document).ready(function(){
$('#banner-carousel').owlCarousel({
loop:true,
margin:10,
nav:true,
dots:true,
autoplay:true,
responsive:{
    0:{
        items:1
    },
    600:{
        items:1
    },
    1000:{
        items:1
    }
},
    navText: ['<img src="themes/artviet/assets/images/next.png">','<img src="themes/artviet/assets/images/prev.png">']
});
$('#studio-carousel').owlCarousel({
loop:true,
margin:10,
nav:true,
dots:true,
autoplay:true,
responsive:{
    0:{
        items:1
    },
    600:{
        items:1
    },
    1000:{
        items:1
    }
},
    navText: ['<img src="themes/artviet/assets/images/next.png">','<img src="themes/artviet/assets/images/prev.png">']
});
$('#slider-doitac').owlCarousel({
loop:true,
nav:false,
dots:false,
autoplay:true,
responsive:{
    0:{
        items:2
    },
    600:{
        items:3
    },
    1000:{
        items:4
    }
},

});
});
$(document).ready(function(){
send_contact();
function send_contact() {
$('#contact-form').submit(function(e){
    e.preventDefault();             
    token = $('#contact-form input[name="_token"]').val();
    $.ajax({
        url: '/lien-he',
        async: true,
        type: "POST",
        data: $(this).serialize(),
        success: function(response) {
            if (response == '1') {
                alert('Gửi liên hệ thành công!');
                location.href = '/lien-he';                   
            } else {
                alert('Gửi lỗi!');
                location.href = '/lien-he';
            }
        }
    });
});
}
});