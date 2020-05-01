using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// Нейронная сеть. Класс описываюший одну особь генетического алгоритма
public class NeyroNet : MonoBehaviour {
    
    public float Аctivity;
    public float Mobility;
    public float Vision;

    public String Name;
    public String NameFather;
    public String NameMоther;

    public int NumberKills = 0;
    public int LifeTime = 0;
    
    private int NumberInputs;
    private int NumberHiddenNeurons;
    private int NumberOutputs;

    private float[] Inputs;
    private float[] HiddenNeurons;
    private float[] Outputs;

    public float[,] InputMatrixWeight;
    public float[,] OutputMatrixWeight; 

    public NeyroNet(int NumberInputs=720, int NumberHiddenNeurons=100, int NumberOutputs=9,
                        String MyName="", String NameFather="", String NameMоther="") {
        this.NameFather = NameFather;
        this.NameMоther = NameMоther;
        this.Name = MyName+"/";
        this.Name += NameFather;
        this.Name += "/";
        this.Name += NameMоther;
        if(NameMоther.Length == 0 && NameFather.Length == 0) this.Name += "ClanFounder";

        // Коэффиченты или параметры особи. Её "Лень", "Активность" и "Зрение". 
        // Отвечают за скорость выполнения действия и за вероятность его выполнения и 
        // за зону обнаружения объектов соответственно.
        this.Аctivity = this.RandomFloat(0.0f, 1.0f);
        this.Mobility = this.RandomFloat(0.5f, 1.5f);
        this.Vision = this.RandomFloat(0.5f, 1.5f);

        this.NumberInputs = NumberInputs +1;
        this.NumberHiddenNeurons = NumberHiddenNeurons; 
        this.NumberOutputs = NumberOutputs;

        this.Inputs = this.MakeArray(this.NumberInputs, 1.0f);
        this.HiddenNeurons = this.MakeArray(this.NumberHiddenNeurons, 1.0f);
        this.Outputs = this.MakeArray(this.NumberOutputs, 1.0f);

        // Матрицы коэффицентов нейронной сети
        this.InputMatrixWeight = this.MakeMatrix(this.NumberInputs, this.NumberHiddenNeurons);
        this.OutputMatrixWeight = this.MakeMatrix(this.NumberHiddenNeurons, this.NumberOutputs);

        // Рандомные коэффиценты вначале.
        for(int i=0; i<this.NumberInputs; i++) {
            for(int j=0; j<this.NumberHiddenNeurons; j++) { 
                this.InputMatrixWeight[i, j] = this.RandomFloat(-1.0f, 1.0f); 
            }
        }
        for(int i=0; i<this.NumberHiddenNeurons; i++) {
            for(int j=0; j<this.NumberOutputs; j++) { 
                this.OutputMatrixWeight[i, j] = this.RandomFloat(-1.0f, 1.0f); 
            }
        }
    }

    // "Предсказание" или получение действия сети в зависимости от входных данных
    public float[] Prediction(float[] Inputs) {
        for(int i=0; i<this.NumberInputs-1; i++) { this.Inputs[i] = Inputs[i]; }

        for(int i=0; i<this.NumberHiddenNeurons; i++) {
            float Sum = 0.0f;
            for(int j=0; j<this.NumberInputs; j++) {
                Sum += this.Inputs[j]*this.InputMatrixWeight[j, i];
            }
            this.HiddenNeurons[i] = this.ActivationFunction(Sum);
        }
        for(int i=0; i<this.NumberOutputs; i++) {
            float Sum = 0.0f;
            for(int j=0; j<this.NumberHiddenNeurons; j++) {
                Sum += this.HiddenNeurons[j]*this.OutputMatrixWeight[j, i];
            }
            this.Outputs[i] = this.ActivationFunction(Sum);
        }

        return this.Outputs;
    }

    private float RandomFloat(float From, float To) { 
        return UnityEngine.Random.Range(From, To); 
    }

    // Создание массива
    private float[] MakeArray(int Long, float Fill=0.0f) {
        float[] OutputArray = new float[Long];
        for(int i=0; i<Long; i++) { OutputArray[i] = Fill; }
        return OutputArray;
    }

    // Создание матрицы
    private float[,] MakeMatrix(int Height, int Width, float fill=0.0f) {
        float[,] OutputMatrix = new float[Height, Width];
        for(int i=0; i<Height; i++) {
            for(int j=0; j<Width; j++) { OutputMatrix[i, j] = fill; }
        }
        return OutputMatrix;
    }

    // Функция активации
    private float ActivationFunction(float x) { 
        return (float) Math.Tanh((double) x); 
    }
}

// Класс описывающий генетический алгоритм
public class GeneticAlgorithm : MonoBehaviour {
    public NeyroNet[] Persons; // Массив где храняться все особи 
    public int EraNumber = 1;
    
    public GeneticAlgorithm() {
        this.Persons = new NeyroNet[10];
        for(int i=0; i<10; i++) { this.Persons[i] = new NeyroNet(720, 100, 9, "1/"+((i+1).ToString())); }
    }

    // Метод для скрещивания двух чисел от двух разных особей.
    // В этом метоже реализованна мутация и параметр коэффицент генов.
    private float GeneСalculation(float Gene1, float Gene2, float GeneK, 
                                    float MutationsK=0.1f, float MutationsFrom=-1f, float MutationsTo=1f) { 
        if(UnityEngine.Random.Range(0, 1f) < MutationsK) 
            return UnityEngine.Random.Range(MutationsFrom, MutationsTo);
        else 
            return (Gene1*GeneK) + (Gene2*(1-GeneK)); 
    }

    // Рождение новой особи от двух других
    public NeyroNet Birth(NeyroNet Person1, NeyroNet Person2, int SerialNumber=0, int EraNumber=1, float MutationsK=0.1f, 
                            int NumberInputs=720, int NumberHiddenNeurons=100, int NumberOutputs=9) {
        NeyroNet OutputPerson = new NeyroNet(NumberInputs, NumberHiddenNeurons, NumberOutputs, 
                                                EraNumber+"/"+SerialNumber, Person1.Name.Split('/')[1], Person2.Name.Split('/')[1]);

        // Коэффичент ген. Лучшая особь будет иметь перевес перед зудшей в генах будующего потомства.
        float GeneK;
        GeneK = 0.5f; //(заглушка) пока 0.5 потом будет формула 

        for(int i=0; i<Person1.InputMatrixWeight.GetLength(0); i++) {
            for(int j=0; j<Person1.InputMatrixWeight.GetLength(1); j++) {
                OutputPerson.InputMatrixWeight[i, j] = 
                    GeneСalculation(Person1.InputMatrixWeight[i, j], Person2.InputMatrixWeight[i, j], GeneK, MutationsK);
            }    
        }
        for(int i=0; i<Person1.OutputMatrixWeight.GetLength(0); i++) {
            for(int j=0; j<Person1.OutputMatrixWeight.GetLength(1); j++) {
                OutputPerson.OutputMatrixWeight[i, j] = 
                    GeneСalculation(Person1.OutputMatrixWeight[i, j], Person2.OutputMatrixWeight[i, j], GeneK, MutationsK);
            }    
        }

        OutputPerson.Аctivity = GeneСalculation(Person1.Аctivity, Person2.Аctivity, GeneK, MutationsK);
        OutputPerson.Mobility = GeneСalculation(Person1.Mobility, Person2.Mobility, GeneK, MutationsK, 0.5f, 1.5f);
        OutputPerson.Vision = GeneСalculation(Person1.Vision, Person2.Vision, GeneK, MutationsK, 0.5f, 1.5f);

        return OutputPerson; // Новая особь!
    }

    // Создание новых особей из лучших. Новая эпоха обучения.
    public NeyroNet[] NewEra() {
        this.EraNumber++;
        this.Sort();

        this.Persons[4] = this.Birth(this.Persons[0], this.Persons[1],  5, this.EraNumber);
        this.Persons[5] = this.Birth(this.Persons[1], this.Persons[2],  6, this.EraNumber);
        this.Persons[6] = this.Birth(this.Persons[2], this.Persons[3],  7, this.EraNumber);
        this.Persons[7] = this.Birth(this.Persons[0], this.Persons[2],  8, this.EraNumber);
        this.Persons[8] = this.Birth(this.Persons[0], this.Persons[3],  9, this.EraNumber);
        this.Persons[9] = this.Birth(this.Persons[1], this.Persons[3], 10, this.EraNumber);

        return this.Persons;
    }

    // Выводит информацию о текущей эре в текущий момент времени.
    public String[] GetStatistics() {
        String[] OutputStatistics = new String[this.Persons.Length+1];
        OutputStatistics[0] = "ERA - "+EraNumber + " Persons - "+this.Persons.Length;
        for(int i=0; i<this.Persons.Length; i++) 
            OutputStatistics[i+1] = this.Persons[i].Name + " - " + 
                                this.Persons[i].LifeTime*(this.Persons[i].NumberKills+1) + " Score";
        return OutputStatistics;
    }

    // Сортировка особей по "качеству", которое зависит от 
    // времени жизни и количетсва убитых особей 
    public NeyroNet[] Sort() {
        while(true) {
            bool NotSorted = false;
            for(int i=0; i<this.Persons.Length-1; i++) {
                if((this.Persons[i].LifeTime * (this.Persons[i].NumberKills+1)) <
                        (this.Persons[i+1].LifeTime * (this.Persons[i+1].NumberKills+1))) {
                            // Сортировка пузырьком
                    NeyroNet PersonTmp = Persons[i];
                    this.Persons[i] = this.Persons[i+1];
                    this.Persons[i+1] = PersonTmp;
                    NotSorted = true;
                }
            }
            if(NotSorted == false) break;
        }

        return this.Persons;
    }

    void Start() {
        GeneticAlgorithm test = new GeneticAlgorithm();
        test.Sort();
        test.NewEra();
        String[] test2 = new String[11];
        test2 = test.GetStatistics();
        for(int i=0; i<11; i++) print(test2[i]);
    }

}