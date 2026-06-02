package main

import (
    "github.com/gin-gonic/gin"
    "github.com/google/uuid"
    "net/http"
)

type Review struct {
    ID      string `json:"id"`
    GuestID string `json:"guest_id"`
    RoomID  string `json:"room_id"`
    Rating  int    `json:"rating"`
    Comment string `json:"comment"`
}

var reviews = []Review{}

func main() {
    r := gin.Default()

    r.POST("/reviews", func(c *gin.Context) {
        var review Review
        if err := c.BindJSON(&review); err != nil {
            c.JSON(http.StatusBadRequest, gin.H{"error": "Invalid request"})
            return
        }
        review.ID = uuid.New().String()[:8]
        reviews = append(reviews, review)
        c.JSON(http.StatusCreated, review)
    })

    r.GET("/reviews/room/:room_id", func(c *gin.Context) {
        roomID := c.Param("room_id")
        var result []Review
        for _, r := range reviews {
            if r.RoomID == roomID {
                result = append(result, r)
            }
        }
        c.JSON(http.StatusOK, result)
    })

    r.Run(":5004")
}
