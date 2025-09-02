package hu.bme.aut.szoftlab.flights.dto;

public class Airline {
	private int id;
	private String from;
	private String to;
	private String currency;
	private double price;

	public Airline() {
	}

	public Airline(int id, String from, String to, String currency, double price) {
		this.id = id;
		this.from = from;
		this.to = to;
		this.currency = currency;
		this.price = price;
	}

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public String getFrom() {
		return from;
	}

	public void setFrom(String from) {
		this.from = from;
	}

	public String getTo() {
		return to;
	}

	public void setTo(String to) {
		this.to = to;
	}

	public String getCurrency() {
		return currency;
	}

	public void setCurrency(String currency) {
		this.currency = currency;
	}

	public double getPrice() {
		return price;
	}

	public void setPrice(double price) {
		this.price = price;
	}
}
