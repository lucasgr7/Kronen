kronenApp.controller('controller',function($scope){
  
  var naipes = [{simbolo: '♣',texto: 'paus'},{simbolo: '♦',texto: 'ouro'},{simbolo: '♥',texto: 'coracao' },{simbolo: '♠',texto: 'espadilha'}];
  var valores = [ //'2','3','4','5','6','7','8','9','10','J',
  'Q','K','A'];

  $scope.valoresSliderOptions = {
    optionsValor: {
      stepsArray: valores
    },
    optionsNaipe: {
      stepsArray: naipes.map((x) =>{
        return x.simbolo
      })
    }
  }
  $scope.cartas = [];
  $scope.jogadores = 2;
  indexCartas = 0;
  $scope.lista = [];
  $scope.cartas_roubadas = [];
  

  gerarBaralho();
  loadCartas();
  gerarPainelJogador();

  function gerarBaralho(){
    // baralho = [];
    // naipes.map((naipe) =>{
    //   valores.map((v) =>{
    //     baralho.push({
    //       naipe: naipe,
    //       valor: v,
    //       especial:false,
    //       index: getUniqueKey(),
    //       isNextOrPrevious : isNextOrPrevious,
    //       drag: true,
    //       valorNumerico: generateNumeric(v)
    //     });
    //   });
    // });
    // for(var x = 0; x < 4; x++){
    //   // baralho.push({
    //   //   naipe: null,
    //   //   valor:'thief', //ladrão
    //   //   especial : true,
    //   //   index: getUniqueKey(),
    //   //   isNextOrPrevious : isNextOrPrevious,
    //   //   drag: true
    //   // });
    //   baralho.push({
    //     naipe: null,
    //     valor:'joker', //coringa
    //     especial : true,
    //     index: getUniqueKey(),
    //     isNextOrPrevious : isNextOrPrevious,
    //     drag: true
    //   });
    // }

  }
  $scope.setCarta = function(carta){
    if(carta.especial){
      if(carta.valor === 'thief'){
        $scope.isHoldingThief = true;
      }
      else if(carta.valor === 'joker'){
        $scope.isHoldingJoker = true;
      }
    }else{
      $scope.isHoldingThief = false;
      $scope.isHoldingJoker = false;
    }
    console.log(carta);
  }

  function gerarPainelJogador(){
    $scope.jogadores = [{ id : getUniqueKey() },{id : getUniqueKey()}];
    $scope.jogadores.map(function(jogador){
      jogador.colunas = [];
      jogador.pontuacoes = [];
      jogador.getNaipe = getNaipe;
      jogador.getCarta = getCarta;
      for(var x = 0; x < 3 ;x ++){
        jogador.colunas.push({
          v: x,
          linhas: [
          {
            linha: 0,
            number: getUniqueKey(),
            data: []
          },
            {
              linha: 1,
              number: getUniqueKey(),
              data: []
            },
            {
              linha: 2,
              number: getUniqueKey(),
              data: []
            }
          ]
        });
      }
    });
  }

  function getCartaAleatoria(){
    var posicao = Math.floor(Math.random() * baralho.length);
    var carta = baralho[posicao]
    baralho.splice(posicao, 1);
    console.log(baralho.length);
    return carta;
  }

  function loadCartas(){
    for(var x = 0; x < 5 ; x ++){
      $scope.cartas.push(getCartaAleatoria());
    }
  }
  function generateNumeric(v){
    if(v === 'J'){
      return 11;
    }
    else if(v === 'Q'){
      return 12;
    }
    else if(v === 'K'){
      return 13;
    }
    else if(v === 'A'){
      return 14;
    }else{
      return Number(v);
    }

  }

  function getUniqueKey(){
    indexCartas += 1;
    return indexCartas;
  }

  $scope.isRed = function(carta){
    if(carta == null || carta.naipe == null){
      return false;
    }
    if(carta.naipe.simbolo != null){
      if(carta.naipe.simbolo.includes(['♦']) || carta.naipe.simbolo.includes(['♥']))
        return true;
    }
    else if(carta.naipe.includes(['♦']) || carta.naipe.includes(['♥']))
      return true;
    else
      return false;
  }
  $scope.cardPlaced = function(e){
    console.log(e);
    if($scope.isHoldingThief){
      var posicao_roubada = Number(e.target.dataset.number == null ? 0 : e.target.dataset.number); //posição roubada
      var vitima_id = Number(e.target.dataset.player_id  == null ? 0 : e.target.dataset.player_id); //posição roubada
      var jogador = $scope.jogadores.find((x) => {
        if(x.id === vitima_id){
          return x;
        }
      });
      jogador.colunas.forEach((col) => {
        col.linhas.forEach((lin) => {
          if(lin.number === posicao_roubada){
            $scope.cartas_roubadas = [];
            $scope.cartas_roubadas.push(lin.data[0]);
            lin.data.splice(0,2);
            $scope.isHoldingThief = false;
            $scope.isPlacingThief = true;
          }
        })
      });

    }else if($scope.isPlacingThief){
      $scope.cartas_roubadas = [];
      $scope.isPlacingThief = false;
    }else if($scope.isHoldingJoker){
      var poslicao_coringa = Number(e.target.dataset.number == null ? 0 : e.target.dataset.number); //posição roubada
      var jogador_id = Number(e.target.dataset.player_id  == null ? 0 : e.target.dataset.player_id); //posição roubada
      var jogador = $scope.jogadores.find((x) => {
        if(x.id === jogador_id){
          return x;
        }
      });

      jogador.colunas.forEach((col) => {
        col.linhas.forEach((lin) => {
          if(lin.number === poslicao_coringa){
            $scope.coringaSet = lin.data[0];
            $scope.isHoldingJoker = false;
          }
        })
      });
      console.log(jogador);
    }
    $scope.setCoringa = function(){
      $scope.coringaSet.valor = jogador.pick.valor;
      $scope.coringaSet.valorNumerico = generateNumeric(jogador.pick.valor);
      $scope.coringaSet.naipe = convertNaipeToObject(jogador.pick.naipe);
      $scope.cardPlaced();
    }
    

    $scope.changeCard();
    $scope.jogadores.forEach(jogador => {
      jogador.pontuacoes = [];
      jogador.totalPontos = 0;
      //Pontuação das naipe 
      validaNaipes(jogador);
      validaTrincaDupla(jogador);
      validaSequencias(jogador);
      jogador.pontuacoes.forEach(p => {
        jogador.totalPontos += p.valor
      })
    });
    return null;
  }

  $scope.changeCard = function(){
    for(var x = 0; x < $scope.cartas.length; x++){
      if(isEmpty($scope.cartas[x])){
        $scope.cartas[x] = getCartaAleatoria();
      }
    }
  }

  //to-do: Implementar pontuações ROYAL STRAIGHT FLUSH
  function isEmpty(obj) {
    for(var key in obj) {
        if(obj.hasOwnProperty(key))
            return false;
    }
    return true;
  }
  function getNaipe(x, y){
    if(this.colunas[y] != null && this.colunas[y].linhas[x] != null && this.colunas[y].linhas[x].data != null && this.colunas[y].linhas[x].data.length > 0){
      var naipe = this.colunas[y].linhas[x].data[0].naipe;
      if(naipe != null)
      {
        if(naipe.simbolo != null){
          return this.colunas[y].linhas[x].data[0].naipe.texto;
        }
      }
      return null;
    }
  }
  function getCarta(x, y){
    if(this.colunas[y] != null && this.colunas[y].linhas[x] != null && this.colunas[y].linhas[x].data != null && this.colunas[y].linhas[x].data.length > 0){
      return this.colunas[y].linhas[x].data[0];
    }
    return {
      valor: null
    };
  }
  function validaNaipes(jogador){
    for(var x = 0; x < 3; x++){
        if(jogador.getNaipe(x,0) != null && jogador.getNaipe(x,0) === jogador.getNaipe(x, 1) && jogador.getNaipe(x, 1) === jogador.getNaipe(x, 2)){
          jogador.pontuacoes.push({
            flush: true,
            valor: 350
          })
        }
        if(jogador.getNaipe(0,x) != null && jogador.getNaipe(0,x) === jogador.getNaipe(1, x) && jogador.getNaipe(1, x)=== jogador.getNaipe(2, x)){
          jogador.pontuacoes.push({
            flush: true,
            valor: 350
          })
        }
      }
      if(jogador.getNaipe(0,0) != null && jogador.getNaipe(0,0) === jogador.getNaipe(1, 1) && jogador.getNaipe(1, 1)=== jogador.getNaipe(2, 2)){
          jogador.pontuacoes.push({
            flush: true,
            valor: 350
          })
        }
        if(jogador.getNaipe(0,2) != null && jogador.getNaipe(0,2) === jogador.getNaipe(1, 1) && jogador.getNaipe(1, 1)=== jogador.getNaipe(2, 0)){
          jogador.pontuacoes.push({
            flush: true,
            valor: 350
          })
        }
  }
  function validaTrincaDupla(jogador){
    for(var x = 0; x < 3; x++){
      var carta_a = jogador.getCarta(x, 0);
      var carta_b = jogador.getCarta(x, 1);
      var carta_c = jogador.getCarta(x, 2);
      // Vertical
      var cartasValidas = isCard(carta_a) && isCard(carta_b) && isCard(carta_c);
      if(cartasValidas && carta_a.valor == carta_b.valor && carta_b.valor === carta_c.valor){
        jogador.pontuacoes.push({
          trinca: true,
          valor: 750
        })
      }else if(isTwoCardsEqual(carta_a,carta_b,carta_c)){
        jogador.pontuacoes.push({
          dupla: true,
          valor: 200
        })
      }
      
      carta_a = jogador.getCarta(0, x);
      carta_b = jogador.getCarta(1, x);
      carta_c = jogador.getCarta(2, x);
      var cartasValidas = isCard(carta_a) && isCard(carta_b) && isCard(carta_c);
      // Horizontal
      if(cartasValidas && carta_a.valor == carta_b.valor && carta_b.valor === carta_c.valor){
        jogador.pontuacoes.push({
            trinca: true,
            valor: 750
          })
      }else if(isTwoCardsEqual(carta_a,carta_b,carta_c)){
        jogador.pontuacoes.push({
          dupla: true,
          valor: 200
        })
      }
    }
    //Diagonal A
    cartasValidas = isCard(jogador.getCarta(0,0)) && isCard(jogador.getCarta(1,1)) && isCard(jogador.getCarta(2,2));
    var carta_a = jogador.getCarta(0, 0);
    var carta_b = jogador.getCarta(1, 1);
    var carta_c = jogador.getCarta(2, 2);
    if(cartasValidas && carta_a.valor === carta_b.valor && carta_b.valor === carta_c.valor){
        jogador.pontuacoes.push({
          trinca: true,
          valor: 750
        })
    }else if(isTwoCardsEqual(carta_a,carta_b,carta_c)){
        jogador.pontuacoes.push({
          dupla: true,
          valor: 200
        })
    }
    //Diagonal B
    cartasValidas = isCard(jogador.getCarta(0,2)) && isCard(jogador.getCarta(1,1)) && isCard(jogador.getCarta(2,0));
    carta_a = jogador.getCarta(0, 2);
    carta_b = jogador.getCarta(1, 1);
    carta_c = jogador.getCarta(2, 0);
    if(cartasValidas && carta_a.valor === carta_b.valor && carta_b.valor === carta_c.valor){
        jogador.pontuacoes.push({
          trinca: true,
          valor: 750
        })
      }else if(isTwoCardsEqual(carta_a,carta_b,carta_c)){
        jogador.pontuacoes.push({
          dupla: true,
          valor: 200
        })
      }
  }
  function validaSequencias(jogador){
    for(var x = 0; x < 3; x++){
      var carta_a = jogador.getCarta(x, 0);
      var carta_b = jogador.getCarta(x, 1);
      var carta_c = jogador.getCarta(x, 2);
      // Vertical
      if(isCard(carta_a) && isCard(carta_b) && isCard(carta_c)){
        if(isSequence([carta_a, carta_b, carta_c])) {
          if(isRoyal([carta_a,carta_b,carta_c])){
            jogador.pontuacoes.push({
              royal_straight_flush: true,
              valor: 1150
            });
          }else{
            jogador.pontuacoes.push({
              straight: true,
              valor: 450
            });
          }
        }
      }
      carta_a = jogador.getCarta(0, x);
      carta_b = jogador.getCarta(1, x);
      carta_c = jogador.getCarta(2, x);
      // Horizontal
      if(isCard(carta_a) && isCard(carta_b) && isCard(carta_c)){
        if(isSequence([carta_a, carta_b, carta_c])){
          if(isRoyal([carta_a,carta_b,carta_c])){
            jogador.pontuacoes.push({
              royal_straight_flush: true,
              valor: 1150
            });
          }else{
            jogador.pontuacoes.push({
              straight: true,
              valor: 450
            });
          }
        }
      }
    }
    //Diagonal A
    cartasValidas = isCard(jogador.getCarta(0,0)) && isCard(jogador.getCarta(1,1)) && isCard(jogador.getCarta(2,2));
    var carta_a = jogador.getCarta(0, 0);
    var carta_b = jogador.getCarta(1, 1);
    var carta_c = jogador.getCarta(2, 2);
    if(cartasValidas && isSequence([carta_a, carta_b, carta_c])){
      if(isRoyal([carta_a,carta_b,carta_c])){
        jogador.pontuacoes.push({
          royal_straight_flush: true,
          valor: 1150
        });
      }else{
        jogador.pontuacoes.push({
          straight: true,
          valor: 450
        });
      }
    }
    //Diagonal B
    cartasValidas = isCard(jogador.getCarta(0,2)) && isCard(jogador.getCarta(1,1)) && isCard(jogador.getCarta(2,0));
    carta_a = jogador.getCarta(0, 2);
    carta_b = jogador.getCarta(1, 1);
    carta_c = jogador.getCarta(2, 0);
    if(cartasValidas && isSequence([carta_a, carta_b, carta_c])){
        if(isRoyal([carta_a,carta_b,carta_c])){
          jogador.pontuacoes.push({
            royal_straight_flush: true,
            valor: 1150
          });
        }else{
          jogador.pontuacoes.push({
            straight: true,
            valor: 450
          });
        }
      }
  }
  function convertNaipeToObject(naipe){
    return naipes.filter((x) => {
      if(x.simbolo === naipe){
        return x;
      }
    })[0];
  }
  function isRoyal(cartas){
    var isKing = false;
    var isQueen = false;
    var isAce = false;
    var isSameNaipe = true;
    for(var x = 0; x < cartas.length; x++){
      var c = cartas[x];
      if(x > 0){
        if(c.naipe !== cartas[x-1].naipe)
          isSameNaipe = false;
      }
      if(c.valor === "K"){
        isKing = true;
      }
      else if(c.valor === "Q"){
        isQueen = true;
      }else if(c.valor === "A"){
        isAce = true;
      }
    }
    if(isKing && isQueen && isAce && isSameNaipe){
      return true;
    }
  }
  function isSequence(cartas){
    
    for(var x = 0; x < cartas.length; x++){
      var c = cartas[x];
      if(!!c){
        for(var y = 0; y < cartas.length; y++){
          if(y != x){
            var c2 = cartas[y];
            if(c.isNextOrPrevious(c2)){
              for(var z = 0; z < cartas.length; z++){
                if(z != x && z != y){
                  c3 = cartas[z];
                  if(c2.isNextOrPrevious(c3) && c2.isNextOrPrevious(c) && c3.valor != c.valor){
                    return true;
                  }else if(c.isNextOrPrevious(c3) && c.isNextOrPrevious(c2) && c3.valor != c2.valor){
                    return true;
                  }
                }
              }
            }
          }
        }
      }
    }
  }
  function isTwoCardsEqual(carta_a,carta_b,carta_c){
    return !!carta_a.valor && carta_a.valor === carta_b.valor ||
        !!carta_b.valor && carta_b.valor === carta_c.valor ||
        !!carta_c.valor && carta_a.valor === carta_c.valor;
  }
  function isNextOrPrevious(carta){
    return this.valorNumerico + 1 == carta.valorNumerico || this.valorNumerico - 1 == carta.valorNumerico;
  }
  function isCard(carta){
    return carta != null && carta.valor != null;
  }
})