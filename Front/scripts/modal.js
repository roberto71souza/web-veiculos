// Controle do slide
(function () {
  var sliderClass = "",
    activeClass = "active";

  $(".modal").on("click", "#nextBtn", function () {
    rotateNext();
  });

  $(".modal").on("click", "#prevBtn", function () {
    var slideAtual = $(sliderClass + " > ." + activeClass),
      slideAnterior = slideAtual.prev();

    if (slideAnterior.length === 0) {
      slideAnterior = $(sliderClass + ">:last");
    }
    slideAtual.removeClass(activeClass);
    slideAnterior.addClass(activeClass);
  });

  function slide(slideName, velocidade) {
    sliderClass = "." + slideName;
    var rotate = setInterval(rotateNext, velocidade);

    $(sliderClass + ">:first").addClass(activeClass);

    $(".modal").on("mouseenter", sliderClass, function () {
      clearInterval(rotate);
    });

    $(".modal").on("mouseleave", sliderClass, function () {
      rotate = setInterval(rotateNext, velocidade);
    });
  }

  function rotateNext() {
    var slideAtual = $(sliderClass + " > ." + activeClass),
      proximoSlide = slideAtual.next();

    if (proximoSlide.length === 0) {
      proximoSlide = $(sliderClass + ">:first");
    }
    slideAtual.removeClass(activeClass);
    proximoSlide.addClass(activeClass);
  }

  slide("slider-inner", 8000);
})();
