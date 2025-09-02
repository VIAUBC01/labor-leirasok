package hu.bme.aut.szoftlab.booking.web;

import hu.bme.aut.szoftlab.booking.dto.PurchaseData;
import hu.bme.aut.szoftlab.booking.dto.TicketData;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api")
public class BookingController {

    @Value("${booking.bonus}")
    double bonusRate;

    @PostMapping("/ticket")
    public PurchaseData buyTicket(@RequestBody TicketData ticketData) {
        return null;
    }
}
