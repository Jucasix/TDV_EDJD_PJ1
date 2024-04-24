"# TDV_EDJD_PJ1" 


# Introdução:

Este projeto teve por base a analise ao código de um jogo com base em Monogame e encontrado em repositório publico no Github.



# Projeto realizado por:

-29579 - Roberto Alvarenga

-29580 - João Carvalho




# Análise: 

Após investigação ao código e ao jogo, tecemos os seguintes comentários.

1. **Experiência de jogo:**

- **Menu:**

	- Os botões estão desalinhados, apesar da sua construção estar adaptada para o uso do rato, mas apenas se pode clicar em determinadas partes dos botões para ativar as funcionalidades e em parte alguma isso é mencionado, levando a que se julgue inicialmente que o jogo pode não estar a funcionar de qualquer modo.

- **Jogabilidade:**

	- Ao jogar verifica-se que inicialmente o jogo começa muito devagar e player consegue fazer saltos enormes, tornando a experiência entediante, mas rapidamente a velocidade aumenta de modo que se torna impossível de jogar. Também verificamos que se perdermos e clicarmos em “Click to Replay” o jogo inicia com os mesmos pontos que tínhamos, mas com mais velocidade, o que é suposto servir como um checkpoint system, mas tendo em conta que é impossível jogar o jogo após perto de 10000 score, isto força o player a fechar e re-abrir o jogo para continuar a jogar.

2.  **Analise ao código:**

	- **Código:**
	
		- O código apresenta uma arquitetura relativamente simples, em que existe um movimento "falso". O player é estático, apenas podendo mover-se no eixo Y. Os obstáculos e a estrada são movidos na direção do player, e no caso da estrada, ela é reposicionada à posição inicial após um certo ponto.
		- Apesar da sua simplicidade, apresenta também vários erros:
		- Existe uma parte do código repetida que soma o score em função do delta time 2 vezes em vez de simplesmente multiplicar por 2 
		- Verifica-se que o código contém excerptos de código que literalmente não fazem nada, pois foram comentados e o jogo compila e corre normalmente.
			
			**Trechos do código que foram comentados por nós!**
			
			* *//This doesn't actually do anything, but i assume the author intended to increase the overall game speed according to milestonePoints, but his game speed is actually just tied to DeltaTime * *
			//I assume its supposed to go like this: Everytime the player reacches the milestone point, the speed increases by 0.05. It then adds 2000 to the next milestone points, preventing it from constantly increasing
            //if (score >= milestonePoints)
            //{
            //    gameSpeedIncrease += 0.05f;                
            //    milestonePoints += 2000;



			* *//Same as before, this code doesn't actually do anything other than sum milestonePoints* *
            //if (score >= milestonePoints)
            //{
            //    roadScrollSpeed += roadScrollSpeed * roadScrollSpeedIncrease;
            //    milestonePoints += 2000;

	- **Classes:**
	
		- Verifica-se apenas a existência de duas Classes a classe principal com o nome “BatmanGame.cs” onde temos os requisitos obrigatórios do Monogame e a lógica do jogo, e a classe “SpikeObstacle”, onde são definidos os obstáculos.	




#Melhorias:

1. **Experiência de jogo:**

- **Menu:**
	
	- A criação de texto na tela inicial que diga ao jogador para usar o rato, e também qual o objetivo do jogo, e correção da posição dos botões.
		
- **Jogabilidade:**	

	- Tornar a experiência mais apelativa melhorando o modo como o jogo faz a aceleração, tomando especial nota à escabilidade linear da velocidade em função de DeltaTime, que devia ser substituida por uma função logarítmica.
	- Acrescentar uma opção de voltar ao menu quando se perde o jogo em vez de ser forçado a retomar o checkpoint.
	- Criação de save/load system que interage com o disco, registo de scores leitos destes, para incentivar ao uso com a vontade chegar ao topo da tabela
	- Incluir outros tipos de obstáculos, especialmente obstáculos que estejam posicionados no ar.

2.  **Analise ao código:**

	- **Código:**
	
		- Eliminar as partes que não estão a fazer nada, ou reutilizar-las para o seu provável propósito inicial,
		- Corrigir erros de programação e geralmente adequar o código a melhores práticas de programação.

	- **Classes:**
	
		- Sobre as classes, não temos muito acrescentar, dado que se trata de um jogo simples.


#Conclusão

- Com este projeto de investigação e analise sobre o trabalho de terceiros, aprendemos em primeiro a grande importância de rever o nosso trabalho e até pedir a sua revisão por outras partes que possam tecer comentários valiosos para a melhoria do mesmo. Permitiu-nos também verificar que os jogos feitos na versão 3.7 não funcionam na versão 3.8 que estamos a usar. Também nos permitiu analisar outros projetos baseados em Monogame e verificar conhecimentos já adquiridos em contexto de aula e como os mesmo são aplicados em outros trabalhos e não só no que estamos a usar como exemplo em aula. O facto de este trabalho ser baseado na pesquisa de um trabalho que tivesse num repositório publico, deu-nos a oportunidade de verificar vários trabalhos e de adquirir maior conhecimento e ideias para o próximo trabalho.  


#Bibliografia:

https://github.com/Bilal-Ozdemir/BatmanGame.git