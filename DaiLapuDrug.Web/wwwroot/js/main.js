

$(function () {

  $(".mobile__menu-btn").click(function () {
    $(".menu__mobile").toggleClass("menu__mobile--active");
  });

  
  const topSlider = new Swiper(".top-slider", {
    freeMode: true,
    spaceBetween: 15,
    slidesPerView: 2.8,
    speed: 500,
    breakpoints: {
      1450: {
        slidesPerView: "7.5"
      },
      1150: {
        slidesPerView: "5.5"
      },
      800: {
        slidesPerView: "4.5"
      }
    },
  });
  const topSlider__desc = new Swiper(".top-slider__desc", {
    freeMode: true,
    slidesPerView: 1.8,
    speed: 500,
    spaceBetween: 10,
    breakpoints: {
      600: {
        slidesPerView: "3.2",
      },
      500: {
        slidesPerView: "2.5"
      }
    }
  });

  $(".top-slider__btn").on("mousedown", function () {
    $(".top-slider__btn").addClass("top-slider__btn--on");
  });
  $(".top-slider__btn").on("mouseup", function () {
    $(".top-slider__btn").removeClass("top-slider__btn--on");
  });
});

$(function () {
  $(".cell__item").click(function () {
    $(this).toggleClass("cell__item--active");
  });
});

$(function () {
  $(".all__top-text-color").click(function () {
    $(this).toggleClass("all__top-text-color--active");
  });
});

$(function () {
  $(".all__top-item--1").click(function (e) {
    $(".all__top-submenu--1").addClass("all__top-submenu--active"),
      $(".all__top-category--1").addClass("all__top-category--1-active");
  });
  $(".all__top-item--2").click(function (e) {
    $(".all__top-submenu--2").addClass("all__top-submenu--active"),
      $(".all__top-category--2").addClass("all__top-category--2-active");
  });
  $(".all__top-item--3").click(function (e) {
    $(".all__top-submenu--3").addClass("all__top-submenu--active"),
      $(".all__top-category--3").addClass("all__top-category--3-active");
  });

  $(document).mouseup(function (e) {
    var div1 = $(".all__top-submenu--1");
    if (!div1.is(e.target) && div1.has(e.target).length === 0) {
      div1.removeClass("all__top-submenu--active"),
        $(".all__top-category--1").removeClass("all__top-category--1-active");
    }
    var div2 = $(".all__top-submenu--2");
    if (!div2.is(e.target) && div2.has(e.target).length === 0) {
      div2.removeClass("all__top-submenu--active"),
        $(".all__top-category--2").removeClass("all__top-category--2-active");
    }
    var div3 = $(".all__top-submenu--3");
    if (!div3.is(e.target) && div3.has(e.target).length === 0) {
      div3.removeClass("all__top-submenu--active"),
        $(".all__top-category--3").removeClass("all__top-category--3-active");
    }
  });
});
