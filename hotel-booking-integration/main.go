package main

import (
    "github.com/gin-gonic/gin"
    "net/http"
    "os"
)

func main() {
    r := gin.Default()
    port := os.Getenv("PORT")
    if port == "" {
        port = "5003"
    }

    r.POST("/payments", func(c *gin.Context) {
        c.JSON(http.StatusOK, gin.H{
            "status":     "SUCCESS",
            "payment_id": "pay_123",
        })
    })

    r.GET("/health", func(c *gin.Context) {
        c.JSON(http.StatusOK, gin.H{"status": "ok"})
    })

    r.Run(":" + port)
}
