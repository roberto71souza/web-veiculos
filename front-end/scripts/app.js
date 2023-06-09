const baseUrl = "http://localhost:5000/Veiculos/";
const baseUrlImg = "http://localhost:5000/Arquivos/Imagens/";

$(document).ready(async (event) => {
  var htmlVeiculos = "",
    paginacao = "",
    proximaPagina = sessionStorage.getItem("paginaAtual"),
    containerVeiculo = $(".container-veiculos"),
    containerPaginacao = $(".container-paginacao");

  proximaPagina = proximaPagina !== null ? proximaPagina : 1;

  var response = await getVeiculos(baseUrl, proximaPagina);

  if (response) {
    htmlVeiculos = htmlContainerVeiculos(response.data);

    paginacao = htmlPaginacao(
      response.paginacao.paginaAtual,
      response.paginacao.totalPaginas
    );

    sessionStorage.removeItem("valorBusca");
    sessionStorage.removeItem("paginaAtual");

    sessionStorage.setItem("paginaAtual", response.paginacao.paginaAtual);

    windowResize();

    containerVeiculo.append(htmlVeiculos);
    containerPaginacao.append(paginacao);
  }
});

$(window).resize(
  debounce(() => {
    windowResize();
  }, 150)
);

$("body").on("click", ".container-paginacao > label ", async function (event) {
  var $this = $(this),
    header = {},
    proximaPaginaValue = $this.text(),
    valorBusca = sessionStorage.getItem("valorBusca"),
    url = "",
    htmlVeiculos = "",
    containerVeiculo = $(".container-veiculos");

  if (valorBusca !== null) {
    url = `${baseUrl}getByModelo/${valorBusca}/${proximaPaginaValue}`;
  } else {
    url = baseUrl;
    header = { proximaPagina: proximaPaginaValue };
  }

  $.ajax({ url: url, method: "GET", headers: header })
    .done(function (response) {
      containerVeiculo.html("");

      htmlVeiculos = htmlContainerVeiculos(response.data);

      sessionStorage.removeItem("paginaAtual");
      sessionStorage.setItem("paginaAtual", response.paginacao.paginaAtual);

      containerVeiculo.append(htmlVeiculos);
    })
    .fail(function (error) {
      console.error(error);
    })
    .always(function () {
      $(".container-paginacao > label ").removeClass("active");
      $this.addClass("active");
      $("html, body").animate(
        {
          scrollTop: 0,
        },
        700
      );
    });
});

$("#input-pesquisa").keypress(function (e) {
  if (e.which === 13) {
    $("#button-buscar").click();
  }
});

$("#button-buscar").on("click", function (event) {
  var valorBusca = $("#input-pesquisa").val(),
      containerPaginacao = $(".container-paginacao"),
      url = `${baseUrl}getByModelo/${valorBusca}/1`,
      paginacao = "";

  if (valorBusca !== "") {
    $.ajax({ url: url, method: "GET" })
      .done(async function (response, textStatus, xhr) {
        var containerVeiculo = $(".container-veiculos"),
          htmlVeiculos = "";

        if (xhr.status === 204) {
          response = await getVeiculos(baseUrl);

          htmlVeiculos = htmlContainerVeiculos(response.data);

          alert("Busca não encontrada");
        } else {
          htmlVeiculos = htmlContainerVeiculos(response.data);

          sessionStorage.setItem("valorBusca", valorBusca);
          $("#button-cancelar").css("display", "block");
          $("#button-buscar").css("display", "none");
        }

        paginacao = htmlPaginacao(
          response.paginacao.paginaAtual,
          response.paginacao.totalPaginas
        );

        containerVeiculo.html("");
        containerPaginacao.html("");

        containerVeiculo.append(htmlVeiculos);
        containerPaginacao.append(paginacao);
      })
      .fail(function (error) {
        console.error(error);
      });
  }
});

$("#button-cancelar").on("click", async function (event) {
  var response = await getVeiculos(baseUrl),
      htmlPaginaçao = "",
      htmlVeiculos = "",
      containerVeiculos = $(".container-veiculos"),
      containerPaginacao = $(".container-paginacao");

  containerVeiculos.html("");
  containerPaginacao.html("");

  htmlVeiculos = htmlContainerVeiculos(response.data);
  htmlPaginaçao = htmlPaginacao(
    response.paginacao.paginaAtual,
    response.paginacao.totalPaginas
  );

  containerVeiculos.append(htmlVeiculos);
  containerPaginacao.append(htmlPaginaçao);

  sessionStorage.removeItem("paginaAtual");
  sessionStorage.removeItem("valorBusca");

  $(this).css({ display: "none" });
  $("#button-buscar").css({ display: "block" });
  $("#input-pesquisa").val("");
});

$("#btn-open-modal-adicionar").on("click", function (event) {
  var modal = $(".modal"),
      contentUploadImagens = `<div class="content-upload-imagens">
                                    <button>
                                      <label for="input-file">Escolher imagens</label>
                                    </button>
                                    <input id="input-file" type="file" multiple />
                                    <div class="msg-info"></div>
                              </div>`,
      button = `<button id="modal-btn-salvar">OK</button>`;

  modal.html("");

  let veiculoNullObj = {
    nomeProprietario: "",
    modeloVeiculo: "",
    fabricanteVeiculo: "",
    anoVeiculo: "",
    corVeiculo: "",
    estado: "",
    cidade: "",
    informacoesGerais: "",
    arquivos: null,
  };

  var modalContent = htmlModalVeiculo(
    contentUploadImagens,
    veiculoNullObj,
    button
  );

  modal.prepend(modalContent);
  openModal();
});

$(".modal").on("change", "input#input-file", function (event) {
  var inpFile = $("#input-file"),
      fileNameList = [],
      txtMsg = "",
      elementoP = $("<p></p>"),
      htmlMsgErro = $(".content-upload-imagens > .msg-info ");

  htmlMsgErro.html("");

  if (inpFile[0].files.length <= 5) {
    for (const file of inpFile[0].files) {
      fileNameList.push(file.name);
    }

    txtMsg = fileNameList.join(" || ");

    elementoP.append(`<b>Arquivos selecionados:</b> ${txtMsg}`);
  } else {
    txtMsg = "É possivel escolher somente 5 imagens";

    elementoP.text(txtMsg).css({ color: "red" });
  }

  htmlMsgErro.html(elementoP);
});

$(".modal").on("click", "#modal-btn-salvar", function (event) {
  event.preventDefault();
  var inputFile = $("#input-file"),
    inputs = $(".content-adicionar input,textarea"),
    formData = new FormData();

  if (inputFile[0].files.length === 0) {
    alert("Adicione imagens para cadastrar");
    return;
  }

  if (inputFile[0].files.length > 5) {
    alert("É possivel escolher somente 5 imagens");
    return;
  }

  for (const file of inputFile[0].files) {
    formData.append("formfile", file);
  }

  for (const input of inputs) {
    formData.append(`veiculo.${input.name}`, input.value);
  }

  $.ajax({
    url: baseUrl,
    method: "POST",
    data: formData,
    processData: false,
    contentType: false,
  })
    .done(function (response, textStatus, xhr) {
      alert(xhr.responseText);
      closeModal();
      window.location.reload();
    })
    .fail(function (responseError) {
      let response = responseError.responseJSON ? responseError.responseJSON : responseError.responseText;

      if (response.error) {
        validaFormulario(inputs, response.error);
      } else {
        console.error(response);
      }
    });
});

$(document).on(
  "click",
  ".btn-detalhes-veiculo, .item-lancamentos>img",
  function (event) {
    var modal = $(".modal"),
      idVeiculo = $(this).attr("data-veiculo-id"),
      url = `${baseUrl}getById/${idVeiculo}`,
      contentSlider = "",
      modalContent = "";

    modal.html("");

    $.ajax({ url: url, method: "GET" })
      .done(function (response) {
        contentSlider = htmlContentSlider(response.arquivos);
        modalContent = htmlModalVeiculo(contentSlider, response, "");

        modal.prepend(modalContent);
        openModal();
      })
      .fail(function (error) {
        console.error(error);
      });
  }
);

function closeModal() {
  $(".modal").removeClass("open");
  $(".modal").addClass("remove");
}

function openModal() {
  $(".modal").removeClass("remove");
  $(".modal").addClass("open");
}

function windowResize() {
  if (window.innerWidth >= 1000) {
    secaoUltimosVeiculos(6);
  } else if (window.innerWidth >= 850) {
    secaoUltimosVeiculos(5);
  } else if (window.innerWidth >= 650) {
    secaoUltimosVeiculos(4);
  } else {
    secaoUltimosVeiculos(3);
  }
}

function secaoUltimosVeiculos(qtdItens) {
  $.ajax({ url: `${baseUrl}getUltimosVeiculos/${qtdItens}`, method: "GET" })
    .done(function (response) {
      if (response) {
        htmlUltimosVeiculosAdicionados(response);
      }
    })
    .fail(function (error) {
      console.error(error);
    });
}

function htmlUltimosVeiculosAdicionados(veiculos) {
  var html = "",
    content = $(".header-display-lancamentos");

  for (const veiculo of veiculos) {
    html += `<div class="item-lancamentos">
                <img title="Clique para saber mais - ${veiculo.modeloVeiculo}"
                  src="${baseUrlImg + veiculo.arquivos[0].nomeArquivo}"
                  data-veiculo-id="${veiculo.id}"
                  alt="${veiculo.fabricanteVeiculo}/${veiculo.modeloVeiculo}" />
            </div>`;
  }
  content.html("");
  content.append(html);
}

async function getVeiculos(url, pagina = 1) {
  return $.ajax({
    url: url,
    method: "GET",
    headers: { proximaPagina: pagina.toString() },
  })
    .done(function (response) {})
    .fail(function (error) {
      console.error(error);
    });
}

function validaFormulario(inputs, listError) {
  let listVeiculoError = listError.veiculoError,
      listArquivosError = listError.arquivosError,
      txtMsg = "",
      elementoP = $("<p><b></b></p>"),
      htmlMsgInfo = $(".content-upload-imagens > .msg-info ");

  if (listArquivosError.length > 0) {
    txtMsg = listArquivosError.filter((f) => f.propriedade !== "idVeiculo")
                              .map((m) => m.erro).join(" | ");
    
    if (txtMsg.length > 0) {
        htmlMsgInfo.html("");
      
        elementoP.text(txtMsg).css({ color: "red" });
  
        htmlMsgInfo.html(elementoP);
      }

  }

  for (const input of inputs) {
    let msgError = "",
        lblMsgError = $(`.lbl-error-${input.name}`),
        result = listVeiculoError.filter((error) => error.propriedade === input.name);

    if (result.length > 0) {
      msgError = result.map((list) => list.erro).join(" | ");

      lblMsgError.text(msgError);
      lblMsgError.addClass("active");
      $(input).addClass("input-error");
    } else {
      lblMsgError.removeClass("active");
      $(input).removeClass("input-error");
    }
  }
}

function htmlPaginacao(paginaAtual, TotalPaginas) {
  var htmlLabel = "";

  for (let i = 0; i < TotalPaginas; i++) {
    htmlLabel += `<label class="${i + 1 == paginaAtual ? "active" : ""}">${
      i + 1
    }</label>`;
  }
  return htmlLabel;
}

function htmlContainerVeiculos(veiculos) {
  var html = "";

  for (var veiculo of veiculos) {
    html += `<div class="item-veiculo">
                <img
                  src="${baseUrlImg + veiculo.arquivos[0].nomeArquivo}"
                  title="${veiculo.modeloVeiculo}"/>
                <div class="info-veiculo">
                  <div class="item-texto">
                    <p>${veiculo.modeloVeiculo}</p>
                    <p>${veiculo.estado}/${veiculo.cidade}</p>
                  </div>
                  <button class="btn-detalhes-veiculo" data-veiculo-id="${
                    veiculo.id
                  }">
                    Ver mais
                  </button>
                </div>
             </div>`;
  }
  return html;
}

function htmlContentSlider(imagens) {
  var htmlImg = "",
    htmlSlider = "";

  for (const imagem in imagens) {
    htmlImg += `<img src="${baseUrlImg + imagens[imagem].nomeArquivo}" 
                  title="${imagens[imagem].legenda}"
                  class="${imagem == 0 ? "active" : ""}" />`;
  }

  htmlSlider = `<div class="slider-outer">
                  <img id="prevBtn" src="img/previous.png" alt="anterior" />
                  <div class="slider-inner">
                    ${htmlImg}
                  </div>
                  <img id="nextBtn" src="img/next.png" alt="proximo" />
                </div>`;
  return htmlSlider;
}

function htmlModalVeiculo(contentImage, veiculo, button) {
  const html = `<div class="modal-content">
                  <div class="upload-imagens">
                    <span class="close-button" onclick="closeModal()"> </span>
                    ${contentImage}
                  </div>

                  <div class="content-adicionar">
                    <div class="item">
                      <label>Nome do proprietario</label>
                      <input name="nomeProprietario" value="${veiculo.nomeProprietario}" type="text" />
                      <div class="content-error-message">
                        <label class="lbl-error-nomeProprietario"></label>
                      </div>
                    </div>

                    <div class="item">
                      <div class="row">
                        <label>Modelo</label>
                        <input name="modeloVeiculo" value="${veiculo.modeloVeiculo}" type="text" />  
                        <div class="content-error-message">
                          <label class="lbl-error-modeloVeiculo"></label>  
                        </div>
                      </div>
                      <div class="row">
                        <label>Fabricante do veiculo</label>
                        <input name="fabricanteVeiculo" value="${veiculo.fabricanteVeiculo}" type="text" /> 
                        <div class="content-error-message">
                          <label class="lbl-error-fabricanteVeiculo"></label>   
                        </div>
                      </div>
                    </div>

                    <div class="item">
                      <div class="row">
                        <label>Ano de fabricação do veiculo</label>
                        <input name="anoVeiculo" value="${veiculo.anoVeiculo}" type="text" />
                        <div class="content-error-message">
                          <label class="lbl-error-anoVeiculo"></label>
                        </div>
                      </div>
                      <div class="row">
                        <label>Cor do veiculo</label>
                        <input name="corVeiculo" value="${veiculo.corVeiculo}" type="text" />
                        <div class="content-error-message">
                          <label class="lbl-error-corVeiculo"></label>
                        </div>
                      </div>
                    </div>

                    <div class="item">
                      <div class="row">
                        <label>Estado</label>
                        <input name="estado" value="${veiculo.estado}" type="text" />
                        <div class="content-error-message">
                          <label class="lbl-error-estado"></label>
                        </div>
                      </div>
                      <div class="row">
                        <label>Cidade</label>
                        <input name="cidade" value="${veiculo.cidade}" type="text" />
                        <div class="content-error-message">
                          <label class="lbl-error-cidade"></label>
                        </div>
                      </div>
                    </div>

                    <div class="item">
                      <label>Outras informações</label>
                      <textarea name="informacoesGerais">${veiculo.informacoesGerais}</textarea>
                    </div>
                    <div class="content-adicionar-footer">
                      ${button}
                    </div>
                  </div>
                </div>`;
  return html;
}
