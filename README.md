"# TDV_EDJD_PJ1" 


#Introdução:

Este projeto teve por base a analise ao código de um jogo com base em Monogame e encontrado em repositório publico no Github.



#Prjeto realizado por:

29580 - João Carvalho
29579 - Roberto Alvarenga



#Analise: 

Após investigação ao código e ao jogo, tecemos os seguintes comentários.

1. **Experiência de jogo:**

- **Menu:**

	- A sua construção está adaptada para o uso do rato, mas apenas se pode clicar em determinadas partes dos botões para ativar as funcionalidades e em parte alguma isso é mencionado, levando a que se julgue inicialmente que o jogo pode não estar a funcionar corretamente.

- **Jogabilidade:**

	- Ao jogar verifica-se que inicialmente o jogo começa muito devagar tornando a experiência entediante, mas rapidamente a velocidade aumenta de modo que se torna impossível de jogar. Também verificamos que se perdermos e clicarmos em “Click to Reply” o jogo inicia com os mesmos pontos que tínhamos, mas com mais velocidade, dando a impressão que nem se quer perdemos.

2.  **Analise ao código:**

	- **Código:**
	
		- Verifica-se que o código contém várias partes que não fazem nada, pois foram comentadas e o jogo corre normalmente. Existe uma parte do código repetida que soma o score em função do delta time 2 vezes em vez de simplesmente multiplicar por 2 e o "movimento" do player é falso, não se move para a frente, só salta, é o resto do jogo que move.
			
			**Trechos do   foram comentados por nós!**
			
			* *//Except it doesnt actually do anything* *
			//Everytime the player reacches the milestone point, the speed increases by 0.05. It then adds 2000 to the next milestone points, preventing it from constantly increasing
            //if (score >= milestonePoints)
            //{
            //    gameSpeedIncrease += 0.05f;                
            //    milestonePoints += 2000;



			* *//This code doesn't actually do anything other than incorrectly sum milestonePoints* *
            //if (score >= milestonePoints)
            //{
            //    roadScrollSpeed += roadScrollSpeed * roadScrollSpeedIncrease;
            //    milestonePoints += 2000;

	- **Classes:**
	
		- Verifica-se apenas a existência de duas Classes a classe principal com o nome “BatmanGame.cs” onde temos os requisitos obrigatórios do Monogame e a classe “SpikeObstacle”.	




#Melhorias:

1. **Experiência de jogo:**

- **Menu:**
	
	- A criação de uma tela inicial que mostrasse ao jogador como se usa o menu e se joga, avançando para o menu com o click no enter, e também qual o objetivo do jogo.
		
- **Jogabilidade:**	

	- Tornar a experiência mais apelativa melhorando o modo como o jogo faz a aceleração, acrescentar uma opção de save, quando perdesse o retry seria começar no inicio, a criação de registo de scores, para incentivar ao uso com a vontade chegar ao topo da tabela e incluir outros tipos de obstáculos.

2.  **Analise ao código:**

	- **Código:**
	
		- Eliminar as partes que não estão a fazer nada e as duplicadas e adequado-lo a melhores práticas de programação.

	- **Classes:**
	
		- Sobre as classes, não temos muito acrescentar, dado que se trata de um jogo simples.


#Conclusão

- Com este projeto de investigação e analise sobre o trabalho de terceiros, aprendemos em primeiro a grande importância de rever o nosso trabalho e até pedir a sua revisão por outras partes que possam tecer comentários valiosos para a melhoria do mesmo. Permitiu-nos também verificar que os jogos feitos na versão 3.7 não funcionam na versão 3.8 que estamos a usar. Também nos permitiu analisar outros projetos baseados em Monogame e verificar conhecimentos já adquiridos em contexto de aula e como os mesmo são aplicados em outros trabalhos e não só no que estamos a usar como exemplo em aula. O facto de este trabalho ser baseado na pesquisa de um trabalho que tivesse num repositório publico, deu-nos a oportunidade de verificar vários trabalhos e de adquirir maior conhecimento e ideias para o próximo trabalho.  


#Bibliografia:

https://github.com/Bilal-Ozdemir/BatmanGame.git