using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoansDatabase", menuName = "Database/Loans Database")]
public class LoansDatabaseSO : ScriptableObject
{
    public List<LoanData> loans;
}

[System.Serializable]
public class LoanData
{
    [field: SerializeField]
    public int Id { get; private set; }

    [field: SerializeField]
    public int Amount { get; private set; }

    [field: SerializeField]
    public int MaxPaymentPerRound { get; private set; }

    [field: SerializeField]
    public int InterestRate { get; private set; }
}
