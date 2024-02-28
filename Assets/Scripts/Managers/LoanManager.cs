using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoanManager : MonoBehaviour
{
    public static LoanManager instance;

    [SerializeField] private LoansDatabaseSO loansDatabase;

    private List<int> loansUsed;
    private List<ActiveLoan> loansActive;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;

        loansUsed = new();
        loansActive = new();
    }

    public void ActivateLoan(int id)
    {
        if (loansUsed.Contains(id)) return;

        LoanData foundLoan = loansDatabase.loans.FirstOrDefault(l => l.Id == id);

        if (foundLoan == null) return;

        loansUsed.Add(id);
        CoinsManager.instance.AddCoins(foundLoan.Amount);

        ActiveLoan newLoan = new(foundLoan);
        loansActive.Add(newLoan);
    }

    public void LoansAutoPayment()
    {
        loansActive.ForEach(l =>
        {
            bool paymentCompleted = l.PayRound();

            if (paymentCompleted)
            {
                loansActive.Remove(l);
            }
        });
    }

    public void LoanFullPay(int id)
    {
        if (!loansUsed.Contains(id)) return;

        ActiveLoan foundLoan = loansActive.FirstOrDefault(l => l.Data.Id == id);

        if (foundLoan == null) return;

        bool paymentDone = foundLoan.PayComplete();

        if (paymentDone)
        {
            loansActive.Remove(foundLoan);
        }
    }
}

public class ActiveLoan
{
    public LoanData Data { get; private set; }

    private int coinsPaid = 0;
    private int totalToPay = 0;

    public ActiveLoan(LoanData data)
    {
        Data = data;
        coinsPaid = 0;
        totalToPay = Data.Amount + (Data.Amount * Data.InterestRate / 100);
    }

    public bool PayRound()
    {
        CoinsManager.instance.PayWithNegatives(Data.MaxPaymentPerRound);

        coinsPaid += Data.MaxPaymentPerRound;

        return coinsPaid >= totalToPay;
    }

    public bool PayComplete()
    {
        int remainingAmount = coinsPaid - totalToPay;

        return CoinsManager.instance.RemoveCoins(remainingAmount);
    }
}
