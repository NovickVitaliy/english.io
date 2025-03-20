from flask import Flask, Response, request
from gtts import gTTS
from io import BytesIO

app = Flask(__name__)

def generate_audio_stream(text, lang="en"):
    """
    Generate audio from text using gTTS and yield it as a stream.
    """
    audio_buffer = BytesIO()
    
    tts = gTTS(text=text, lang=lang)
    tts.write_to_fp(audio_buffer)
    
    audio_buffer.seek(0)
    
    while chunk := audio_buffer.read(1024):
        yield chunk

@app.route("/api/tts", methods=["GET"])
def text_to_speech():
    text = request.args.get("text")
    if not text:
        return Response("Missing 'text' parameter", status=400)

    lang = request.args.get("lang", "en")

    try:
        return Response(
            generate_audio_stream(text, lang),
            mimetype="audio/mp3",
            headers={
                "Content-Disposition": "inline; filename=tts.mp3",
                "Accept-Ranges": "bytes"
            }
        )
    except Exception as e:
        return Response(f"Error generating audio: {str(e)}", status=500)

if __name__ == "__main__":
    app.run(debug=True, host="0.0.0.0", port=5003)