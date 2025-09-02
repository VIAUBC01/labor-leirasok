package hu.bme.aut.szoftlab.bonus.model;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;

@Entity
public class Bonus {

    @Id
    @Column(name = "username")
    private String user;
    private double points;
    
    public Bonus() {
    }
    
    public Bonus(String user, double points) {
        this.user = user;
        this.points = points;
    }
    
    public String getUser() {
        return user;
    }
    public void setUser(String user) {
        this.user = user;
    }
    public double getPoints() {
        return points;
    }
    public void setPoints(double points) {
        this.points = points;
    }
    
}
