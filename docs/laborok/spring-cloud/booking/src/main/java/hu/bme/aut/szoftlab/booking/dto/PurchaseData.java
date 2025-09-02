package hu.bme.aut.szoftlab.booking.dto;

public class PurchaseData {

    private boolean success;
    private double price;
    private double bonusUsed;
    private double bonusEarned;
    
    public boolean isSuccess() {
        return success;
    }
    public void setSuccess(boolean success) {
        this.success = success;
    }
    public double getPrice() {
        return price;
    }
    public void setPrice(double price) {
        this.price = price;
    }
    public double getBonusUsed() {
        return bonusUsed;
    }
    public void setBonusUsed(double bonusUsed) {
        this.bonusUsed = bonusUsed;
    }
    public double getBonusEarned() {
        return bonusEarned;
    }
    public void setBonusEarned(double bonusEarned) {
        this.bonusEarned = bonusEarned;
    }
}
