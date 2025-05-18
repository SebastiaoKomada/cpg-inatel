using UnityEngine;

public class movimentCameraMenu : MonoBehaviour
{
    public float anguloTotalRotacao = 90f; // Ângulo total a ser rotacionado a partir da posição inicial
    public float velocidadeRotacao = 30f;  // Velocidade de rotação (em graus por segundo)
    public float tempoEspera = 2f;        // Tempo de espera em segundos ao atingir cada extremo
    private bool rotacionando = true;     // Começa rotacionando
    private float anguloRotado = 0f;
    private float direcao = 1f; // 1 para "ir", -1 para "voltar"
    private float tempoEsperando = 0f;
    private Quaternion rotacaoInicial;

    void Start()
    {
        rotacaoInicial = transform.rotation; // Guarda a rotação inicial
    }

    void Update()
    {
        if (rotacionando)
        {
            float anguloNestaFrame = velocidadeRotacao * Time.deltaTime;
            float anguloRestante = Mathf.Abs(anguloTotalRotacao) - Mathf.Abs(anguloRotado);

            if (anguloNestaFrame > anguloRestante)
            {
                anguloNestaFrame = anguloRestante;
                rotacionando = false;
                tempoEsperando = 0f; // Reinicia o contador de espera
            }

            transform.Rotate(Vector3.up * direcao * anguloNestaFrame, Space.World);
            anguloRotado += anguloNestaFrame;
        }
        else
        {
            tempoEsperando += Time.deltaTime;
            if (tempoEsperando >= tempoEspera)
            {
                direcao *= -1f; // Inverte a direção para o movimento de volta
                anguloRotado = 0f; // Reinicia o ângulo rotacionado
                rotacionando = true; // Começa a rotacionar novamente
            }
        }
    }

    // Opcional: Você ainda pode ter uma função para iniciar/parar manualmente se precisar
    public void AlternarRotacao()
    {
        rotacionando = !rotacionando;
        if (rotacionando)
        {
            anguloRotado = 0f;
            tempoEsperando = 0f;
        }
    }
}
