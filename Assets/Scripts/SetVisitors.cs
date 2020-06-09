using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SetVisitors : MonoBehaviour
{
    [SerializeField] private GameObject visitorPrefab;

    [SerializeField] private GameObject chairPrefab;

    [SerializeField] private float visitorsSpeed;
    
    [SerializeField] private float chance;
    
    [SerializeField] private int chairsCount;
    public int ChairsCount
    {
        get => chairsCount;
        set => chairsCount = value;
    }

    [SerializeField] private int visitorsCount;
    public int VisitorsCount
    {
        get => visitorsCount;
        set => visitorsCount = value;
    }
    
    
    private Transform[] _chairsPositions;

    private Visitor[] _visitors;

    private List<int> _freeChairs;

    private bool _gameStarted;

    private void Update()
    {
        if(!_gameStarted) return;
        ReadInput();
    }

    private void FixedUpdate()
    {
        if(!_gameStarted) return;
        for (int i = 0; i < _visitors.Length; i++)
        {
            if (!_visitors[i].canMove) continue;
            if (!IsInDestinationPoint(_visitors[i].transform.position, _chairsPositions[_visitors[i].chairNumber].position))
            {
                _visitors[i].transform.position = Vector2.MoveTowards(_visitors[i].transform.position,
                    _chairsPositions[_visitors[i].chairNumber].transform.position, visitorsSpeed * Time.deltaTime);
            }
            else
            {
                _visitors[i].canMove = false;
            }
        }
    }

    public void Initialization()
    {
        _chairsPositions = new Transform[chairsCount];
        _visitors = new Visitor[visitorsCount];
        _freeChairs = new List<int>();
        
        for (int i = 0; i < chairsCount; i++)
        {
            _chairsPositions[i] = Instantiate(chairPrefab,PositionInCircle(3f, 360*i / chairsCount),
                Quaternion.identity).transform;
            _freeChairs.Add(i); 
        }
        
        for (int i = 0; i < visitorsCount; i++)
        {
            _visitors[i] = new Visitor
            {
                transform = Instantiate(visitorPrefab, PositionInCircle(1f, 360 * i / visitorsCount),
                    Quaternion.identity).transform
            };
        }

        _gameStarted = true;
    }

    private void ReadInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Trigger();
        }
    }

    private bool IsInDestinationPoint(Vector2 user, Vector2 target)
    {
        return Math.Abs(Vector2.Distance(user,target)) < 0.01f;
    }

    private void Trigger()
    {
        for (int i = 0; i < visitorsCount; i++)
        {
            if (_visitors[i].chairNumber == -1) ChooseChair(i);
            else if(Random.Range(0, 100) <= chance) ChooseChair(i);
        }
    }

    /// <summary>
    /// Выбирает стул для посетителя, занимает его и освобождает предыдущий
    /// </summary>
    /// <param name="visitorNumber"></param>
    private void ChooseChair(int visitorNumber)
    {
        int randomIndex = Random.Range(0, _freeChairs.Count - 1);
        int chairNumber = _freeChairs[randomIndex];
        if(_visitors[visitorNumber].chairNumber!=-1)_freeChairs.Add(_visitors[visitorNumber].chairNumber);
        _visitors[visitorNumber].chairNumber = chairNumber;
        _freeChairs.RemoveAt(randomIndex);
        _visitors[visitorNumber].canMove = true;
    }
    
    /// <summary>
    /// Рассчитывает позицию на окружности, с заданным радиусом и углом
    /// </summary>
    /// <param name="radius">Радиус окружности</param>
    /// <param name="angle">Угол в градусах</param>
    /// <returns></returns>
    private Vector2 PositionInCircle(float radius,float angle)
    {
        var pos = new Vector2
        {
            x = radius * Mathf.Sin(angle * Mathf.Deg2Rad),
            y = radius * Mathf.Cos(angle * Mathf.Deg2Rad)
        };
        return pos;
    }
}

/// <summary>
/// Класс описывающий посетителей
/// </summary>
internal class Visitor
{
    public Transform transform; //Трансформ связанного игрового объекта

    public int chairNumber = -1; //Стул занимаемый посетителем

    public bool canMove; //Маркер разрешающий движение
}
