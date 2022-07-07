const baseUrl = "https://localhost:44307/Veiculos/";
const baseUrlImg = "https://localhost:44307/Recursos/Imagens/";

function Veiculo(
  id,
  nomeProprietario,
  modeloVeiculo,
  fabricanteVeiculo,
  anoVeiculo,
  corVeiculo,
  estado,
  cidade,
  informacoesGerais,
  arquivos
) {
  (this.id = id),
    (this.nomeProprietario = nomeProprietario),
    (this.modeloVeiculo = modeloVeiculo),
    (this.fabricanteVeiculo = fabricanteVeiculo),
    (this.anoVeiculo = anoVeiculo),
    (this.corVeiculo = corVeiculo),
    (this.estado = estado),
    (this.cidade = cidade),
    (this.informacoesGerais = informacoesGerais),
    (this.arquivos = arquivos);
}

$(document).ready(async (event) => {
  var response = await getAllVeiculos(baseUrl),
    htmlVeiculos = "",
    body = $("body");

  htmlVeiculos = htmlGetAllVeiculos(response);

  body.append(htmlVeiculos);
});

$("#input-pesquisa").keypress(function (e) {
  if (e.which === 13) {
    $("#button-buscar").click();
  }
});

$("#button-buscar").on("click", function (event) {
  var valorBusca = $("#input-pesquisa").val(),
    url = `${baseUrl}getByModelo/${valorBusca}`;

  if (valorBusca !== "") {
    $.ajax({ url: url, method: "GET" })
      .done(async function (response, textStatus, xhr) {
        var contentVeiculo = $(".container-info-veiculo"),
          body = $("body"),
          htmlVeiculos = "";

        if (xhr.status === 204) {
          var veiculos = await getAllVeiculos(baseUrl);

          htmlVeiculos = htmlGetAllVeiculos(veiculos);

          alert("Busca não encontrada");
        } else {
          htmlVeiculos = htmlGetAllVeiculos(response);
        }

        for (const content of contentVeiculo) {
          content.remove();
        }

        body.append(htmlVeiculos);
      })
      .fail(function (error) {
        console.log(error);
      });
  }
});

$("#btn-open-modal-adicionar").on("click", function (event) {
  var modal = $(".modal"),
    content = `<div class="content-upload-imagens">
                <button>
                  <label for="input-file">Escolher imagens</label>
                </button>
                <input id="input-file" type="file" multiple />
                <div class="msg-retorno"></div>
              </div>`,
    button = `<button id="modal-btn-salvar">OK</button>`;

  modal.html("");

  let veiculoObj = new Veiculo(0, "", "", "", "", "", "", "", "", null);

  var modalContent = htmlModalVeiculo(content, veiculoObj, button);

  modal.prepend(modalContent);
  modal.removeClass("remove");
  modal.addClass("open");
});

$(".modal").on("change", "input#input-file", function (event) {
  var inpFile = $("#input-file"),
    fileNameList = [],
    txtValue = "",
    elementoP = "",
    htmlMsgErro = $(".content-upload-imagens > .msg-retorno ");

  htmlMsgErro.html("");

  if (inpFile[0].files.length <= 5) {
    for (const file of inpFile[0].files) {
      fileNameList.push(file.name);
    }

    txtValue = fileNameList.join(" || ");
    elementoP = `<p><b>Arquivos selecionados:</b> ${txtValue}</p>`;
  } else {
    txtValue = "É possivel escolher somente 5 imagens";
    elementoP = `<p style="color: red;">${txtValue}</p>`;
  }

  htmlMsgErro.html(elementoP).css({ "font-size": "13px" });
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

  validaInputs(inputs);
  var InputsEhValido = verificaErroInput(inputs);

  if (InputsEhValido) {
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
        console.log(response);
        closeModal();
      })
      .fail(function (error) {
        alert(error.responseText);
        console.log(error);
      });
  }
});

$(document).on("click", ".btn-detalhes-veiculo", function (event) {
  var modal = $(".modal"),
    idVeiculo = $(this).attr("data-veiculo-id"),
    url = `${baseUrl}getById/${idVeiculo}`,
    contentSlider = "",
    modalContent = "";

  modal.html("");

  $.ajax({ url: url, method: "GET" })
    .done(function (response) {
      let veiculoObj = new Veiculo(
        response.id,
        response.nomeProprietario,
        response.modeloVeiculo,
        response.fabricanteVeiculo,
        response.anoVeiculo,
        response.corVeiculo,
        response.estado,
        response.cidade,
        response.informacoesGerais,
        response.arquivos
      );

      contentSlider = htmlContentSlider(veiculoObj.arquivos);
      modalContent = htmlModalVeiculo(contentSlider, veiculoObj, "");

      modal.prepend(modalContent);
      modal.removeClass("remove");
      modal.addClass("open");
    })
    .fail(function (error) {
      console.log(error);
    });
});

function closeModal() {
  $(".modal").removeClass("open");
  $(".modal").addClass("remove");
}

async function getAllVeiculos(url) {
  return $.ajax({ url: url, method: "GET" })
    .done(function (response) {})
    .fail(function (error) {
      console.log(error);
    });
}

function validaInputs(inputs) {
  for (const input of inputs) {
    if (input.value === "" || input.value.trim() === "") {
      $(input).val("");
      $(input).attr("placeholder", "Campo requerido");
      $(input).addClass("input-error");
    } else {
      $(input).removeClass("input-error");
      $(input).removeAttr("placeholder");
    }
  }
}

function verificaErroInput(inputs) {
  for (const input of inputs) {
    if ($(input).hasClass("input-error")) {
      return false;
    }
  }
  return true;
}

function htmlGetAllVeiculos(veiculos) {
  var html = "";

  for (var veiculo of veiculos) {
    html += `<div class="container-info-veiculo">
    <img src="${baseUrlImg + veiculo.arquivos[0].nomeArquivo}"
     title="${veiculo.modeloVeiculo}" />
    <div class="item-info-veiculo">
      <div class="item-texto">
        <p>${veiculo.modeloVeiculo}</p>
        <p>${veiculo.estado}/${veiculo.cidade}</p>
      </div>
      <button class="btn-detalhes-veiculo" data-veiculo-id="${veiculo.id}">
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
                        <img id="prevBtn" src="previous.png" alt="anterior" />
                        <div class="slider-inner">
                          ${htmlImg}
                        </div>
                        <img id="nextBtn" src="next.png" alt="proximo" />
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
                    </div>

                    <div class="item">
                      <div class="row">
                        <label>Modelo</label>
                        <input name="modeloVeiculo" value="${veiculo.modeloVeiculo}" type="text" />    
                      </div>
                      <div class="row">
                        <label>Fabricante do veiculo</label>
                        <input name="fabricanteVeiculo" value="${veiculo.fabricanteVeiculo}" type="text" />    
                      </div>
                    </div>

                    <div class="item">
                      <div class="row">
                        <label>Ano de fabricação do veiculo</label>
                        <input name="anoVeiculo" value="${veiculo.anoVeiculo}" type="text" />
                      </div>
                      <div class="row">
                        <label>Cor do veiculo</label>
                        <input name="corVeiculo" value="${veiculo.corVeiculo}" type="text" />
                      </div>
                    </div>

                    <div class="item">
                      <div class="row">
                        <label>Estado</label>
                        <input name="estado" value="${veiculo.estado}" type="text" />
                      </div>
                      <div class="row">
                        <label>Cidade</label>
                        <input name="cidade" value="${veiculo.cidade}" type="text" />
                      </div>
                    </div>

                    <div class="item">
                      <label>Outras informações</label>
                      <textarea name="informacoesGerais">${veiculo.informacoesGerais}</textarea>
                    </div>
                    ${button}
                  </div>
                </div>`;
  return html;
}
