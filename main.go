package main

import (
    "github.com/gin-gonic/gin"
    "net/http"
)

func main() {
    r := gin.Default()
    
    r.POST("/payments", func(c *gin.Context) {
        c.JSON(http.StatusOK, gin.H{
            "status": "SUCCESS",
            "payment_id": "pay_123",
        })
    })
    
    r.Run(":5003")
}
