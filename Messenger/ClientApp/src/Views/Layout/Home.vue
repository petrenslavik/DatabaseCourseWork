<template>
    <div class="container">
        <div class="card">
            <div>Welcome to fast, secure messenger.</div>
            <div class="buttons">
                <Button text="Register" color="#dc3545"></Button>
                <Button text="Login" color="#dc3545"></Button>
            </div>
        </div>
    </div>
</template>

<script>
    import Button from "@/Views/Components/Button";

    export default {
        name: "Home",
        components: {Button}
    }
</script>

<style scoped>
    .container {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        height: 100%;
    }

    .card{
        padding :10px;
    }

    .buttons {
        margin-top:10px;
        display: flex;
        justify-content: space-evenly;
        width: 100%;
    }

</style>